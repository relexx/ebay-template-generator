/* global React, Panel, Field, Icon */
/* ============================================================
   Stage 1 — Eingabe (icons variant)
   ============================================================ */

function StageInput({ template, content, updateContent, onBack, onNext }) {
  return (
    <div className="stage">
      <div className="page-head">
        <div>
          <h1>Eingabe · Inhalte</h1>
          <p className="page-sub">Fülle die Inhalte für jeden Block aus. Markdown wird im Fließtext unterstützt.</p>
        </div>
        <div className="page-head-actions">
          <div className="btn-stack">
            <button className="btn sm" title="Demo-Daten einfügen"><Icon name="sparkles" size={13}/> Demo</button>
            <button className="btn sm" title="Eingaben zurücksetzen"><Icon name="refresh" size={13}/> Reset</button>
            <button className="btn sm" title="JSON-Schema generieren"><Icon name="fileJson" size={13}/> Schema</button>
            <button className="btn sm" title="Inhalte exportieren"><Icon name="upload" size={13}/> Export</button>
            <button className="btn sm" title="Inhalte importieren"><Icon name="download" size={13}/> Import</button>
          </div>
        </div>
      </div>

      <div className="content-grid">
        {/* Header */}
        <Panel title="Header" icon="pin" badge="Pflicht">
          <Field label="Titel">
            <input
              className="input"
              value={content.title}
              onChange={e => updateContent({ title: e.target.value })}
              placeholder="Premium Produkt XYZ-3000"
            />
          </Field>
          <Field label="Untertitel">
            <input
              className="input"
              value={content.subtitle}
              onChange={e => updateContent({ subtitle: e.target.value })}
              placeholder="Hochwertige Qualität | Art.-Nr. XYZ-3000-BK"
            />
          </Field>
        </Panel>

        {/* Mobile summary */}
        <Panel title="Mobile Summary" icon="smartphone" badge={`${(content.summary || '').length} / 800`}>
          <Field label="Zusammenfassung" hint="max. 800 Zeichen">
            <textarea
              className="textarea"
              rows={5}
              maxLength={800}
              value={content.summary}
              onChange={e => updateContent({ summary: e.target.value })}
              placeholder="Kurzer Pitch für mobile Nutzer — wird auch als schema.org/description ausgegeben."
            />
          </Field>
        </Panel>

        {/* Image */}
        {template.blocks.some(b => b.type === 'bild') && (
          <Panel title="Produktbild" icon="image">
            <Field label="Bild-URL oder Base64">
              <div className="row" style={{ gap: 8 }}>
                <input
                  className="input mono"
                  style={{ fontSize: 12 }}
                  value={content.img?.url || ''}
                  onChange={e => updateContent({ img: { ...content.img, url: e.target.value } })}
                  placeholder="https://…"
                />
                <button className="btn icon-only" title="Bild hochladen" aria-label="Bild hochladen">
                  <Icon name="camera" size={14}/>
                </button>
              </div>
            </Field>
            <div className={`dropzone ${content.img?.url ? 'has-image' : ''}`}>
              {content.img?.url ? (
                <img src={content.img.url} alt="" onError={(e) => { e.currentTarget.style.display = 'none'; }} />
              ) : (
                <span>Bild ablegen oder URL einfügen</span>
              )}
            </div>
          </Panel>
        )}

        {/* Description */}
        {template.blocks.some(b => b.type === 'fließtext') && (
          <Panel title="Beschreibung" icon="fileText" badge="Markdown">
            <Field label="Text" hint="**fett** · *kursiv*">
              <textarea
                className="textarea"
                rows={8}
                value={content.description?.text || ''}
                onChange={e => updateContent({ description: { text: e.target.value } })}
                placeholder="**Das Produkt** bietet…"
              />
            </Field>
          </Panel>
        )}

        {/* Highlights */}
        {template.blocks.some(b => b.type === 'stichwortkarten') && (
          <Panel title="Highlights" icon="sparkles" badge="Titel | Beschr.">
            <Field label="Eine Zeile pro Karte">
              <textarea
                className="textarea mono"
                rows={6}
                value={content.highlights?.raw || ''}
                onChange={e => updateContent({ highlights: { raw: e.target.value } })}
                placeholder={'Premium Qualität | Erstklassige Materialien und Verarbeitung\nInnovatives Design | Moderne Optik trifft Funktionalität'}
              />
            </Field>
          </Panel>
        )}

        {/* Tech data */}
        {template.blocks.some(b => b.type === 'datentabelle') && (
          <Panel title="Technische Daten" icon="settings" badge="Eigenschaft | Wert">
            <Field label="Eine Zeile pro Eigenschaft">
              <textarea
                className="textarea mono"
                rows={7}
                value={content.techData?.raw || ''}
                onChange={e => updateContent({ techData: { raw: e.target.value } })}
                placeholder={'Material | Aluminium / Kunststoff\nAbmessungen | 250 × 150 × 80 mm\nGewicht | 450 g'}
              />
            </Field>
          </Panel>
        )}

        {/* Compatibility */}
        {template.blocks.some(b => b.type === 'featurekarten') && (
          <Panel title="Kompatibilität" icon="wrench" badge="Titel | Beschr.">
            <Field label="Eine Zeile pro Karte">
              <textarea
                className="textarea mono"
                rows={5}
                value={content.compatibility?.raw || ''}
                onChange={e => updateContent({ compatibility: { raw: e.target.value } })}
                placeholder={'Windows 10/11 | Plug & Play, keine Treiber nötig\nmacOS 12+ | Vollständig kompatibel'}
              />
            </Field>
          </Panel>
        )}
      </div>

      <div className="stage-foot">
        <button className="btn" onClick={onBack}>
          <Icon name="arrowLeft" size={14} strokeWidth={2.4}/>
          Zurück zum Layout
        </button>
        <button className="btn primary" onClick={onNext}>
          Weiter zur Vorschau
          <Icon name="arrowRight" size={14} strokeWidth={2.4}/>
        </button>
      </div>
    </div>
  );
}

Object.assign(window, { StageInput });

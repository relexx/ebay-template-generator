/* global React, Panel, Field, IconPicker, Icon */
/* ============================================================
   Stage 0 — Layout (icons variant)
   ============================================================ */

const BLOCK_TYPES = [
  { value: 'bild',             label: 'Bild',             icon: 'image' },
  { value: 'fließtext',         label: 'Fließtext',         icon: 'fileText' },
  { value: 'stichwortkarten',   label: 'Stichwort-Karten',  icon: 'sparkles' },
  { value: 'datentabelle',     label: 'Datentabelle',     icon: 'settings' },
  { value: 'featurekarten',    label: 'Feature-Karten',   icon: 'wrench' },
];

function typeLabel(t) {
  return BLOCK_TYPES.find(b => b.value === t)?.label || t;
}

function StageLayout({ template, updateTemplate, selectedBlockId, setSelectedBlockId, onNext }) {
  const [draggedId, setDraggedId] = useState(null);

  const block = template.blocks.find(b => b.id === selectedBlockId) || template.blocks[0];

  const updateBlock = (patch) => {
    updateTemplate({
      blocks: template.blocks.map(b => b.id === block.id ? { ...b, ...patch } : b)
    });
  };

  const addBlock = () => {
    const id = 'block_' + Math.random().toString(36).slice(2, 8);
    const newBlock = { id, title: 'Neuer Block', type: 'fließtext', icon: 'fileText', showTitle: true, align: 'left' };
    updateTemplate({ blocks: [...template.blocks, newBlock] });
    setSelectedBlockId(id);
  };

  const deleteBlock = () => {
    if (template.blocks.length <= 1) return;
    const idx = template.blocks.findIndex(b => b.id === block.id);
    const next = template.blocks.filter(b => b.id !== block.id);
    updateTemplate({ blocks: next });
    setSelectedBlockId(next[Math.min(idx, next.length - 1)].id);
  };

  const onDragStart = (id) => setDraggedId(id);
  const onDragOver = (e, overId) => {
    e.preventDefault();
    if (!draggedId || draggedId === overId) return;
    const from = template.blocks.findIndex(b => b.id === draggedId);
    const to = template.blocks.findIndex(b => b.id === overId);
    if (from < 0 || to < 0) return;
    const next = [...template.blocks];
    const [moved] = next.splice(from, 1);
    next.splice(to, 0, moved);
    updateTemplate({ blocks: next });
  };
  const onDragEnd = () => setDraggedId(null);

  return (
    <div className="stage">
      <div className="page-head">
        <div>
          <h1>Layout · Struktur</h1>
          <p className="page-sub">Definiere die Blöcke, ihre Reihenfolge und das Farbschema deines Templates.</p>
        </div>
        <div className="page-head-actions">
          <div className="btn-stack">
            <button className="btn sm" title="Neues Layout"><Icon name="plus" size={13}/> Neu</button>
            <button className="btn sm" title="Layout duplizieren"><Icon name="copy" size={13}/> Duplizieren</button>
            <button className="btn sm" title="Layout exportieren"><Icon name="upload" size={13}/> Export</button>
            <button className="btn sm" title="Layout importieren"><Icon name="download" size={13}/> Import</button>
          </div>
        </div>
      </div>

      <div className="toolbar">
        <Field label={null}>
          <select className="select" style={{ width: 220 }} value={template.id} onChange={() => {}}>
            <option>{template.name} (Standard)</option>
            <option>+ Neues Layout erstellen…</option>
          </select>
        </Field>
        <div className="divider" />
        <span className="muted" style={{ fontSize: 12 }}>
          {template.blocks.length} Blöcke · zuletzt geändert vor 3 Min.
        </span>
        <div style={{ flex: 1 }} />
        <button className="btn sm danger" title="Layout löschen">
          <Icon name="trash" size={13}/> Löschen
        </button>
      </div>

      <div className="split">
        {/* Left column */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--gap)' }}>
          <Panel title="Layout-Name" icon="ruler">
            <Field>
              <input
                className="input"
                value={template.name}
                onChange={e => updateTemplate({ name: e.target.value })}
              />
            </Field>
          </Panel>

          <Panel
            title="Blöcke"
            icon="blocks"
            badge={`${template.blocks.length}`}
            actions={
              <button className="btn primary sm icon-only" onClick={addBlock} aria-label="Block hinzufügen" title="Block hinzufügen">
                <Icon name="plus" size={13} strokeWidth={2.4}/>
              </button>
            }
          >
            <div className="block-list">
              {template.blocks.map(b => (
                <div
                  key={b.id}
                  className={`block-item ${b.id === block.id ? 'is-active' : ''}`}
                  onClick={() => setSelectedBlockId(b.id)}
                  draggable
                  onDragStart={() => onDragStart(b.id)}
                  onDragOver={(e) => onDragOver(e, b.id)}
                  onDragEnd={onDragEnd}
                  style={{ opacity: draggedId === b.id ? 0.4 : 1 }}
                >
                  <span className="block-handle" aria-hidden="true">
                    <Icon name="gripVert" size={14} strokeWidth={1.6}/>
                  </span>
                  <span className="block-main">
                    <span className="block-emoji" style={{ display: 'inline-grid', placeItems: 'center', width: 16, height: 16, color: 'var(--muted)' }}>
                      <Icon name={b.icon} size={14} strokeWidth={1.75}/>
                    </span>
                    <span className="block-name">{b.title}</span>
                  </span>
                  <span className="block-meta">{typeLabel(b.type)}</span>
                </div>
              ))}
            </div>
          </Panel>

          <Panel title="Farbschema" icon="palette">
            <ColorRow
              label="Primär"
              value={template.colors.primary}
              onChange={(v) => updateTemplate({ colors: { ...template.colors, primary: v } })}
            />
            <ColorRow
              label="Akzent"
              value={template.colors.accent}
              onChange={(v) => updateTemplate({ colors: { ...template.colors, accent: v } })}
            />
            <ColorRow
              label="Hintergrund"
              value={template.colors.bg}
              onChange={(v) => updateTemplate({ colors: { ...template.colors, bg: v } })}
            />
          </Panel>
        </div>

        {/* Right column */}
        <Panel
          title="Block bearbeiten"
          icon="edit"
          badge={block.id.toUpperCase()}
          actions={
            <button className="btn danger sm" onClick={deleteBlock} disabled={template.blocks.length <= 1}>
              <Icon name="trash" size={13}/> Block löschen
            </button>
          }
        >
          <Field label="ID" hint="Wird für JSON-Export verwendet">
            <input className="input mono" value={block.id} onChange={e => updateBlock({ id: e.target.value })} />
          </Field>

          <Field label="Block-Typ">
            <select
              className="select"
              value={block.type}
              onChange={e => {
                const t = e.target.value;
                const def = BLOCK_TYPES.find(x => x.value === t);
                updateBlock({ type: t, icon: def?.icon || block.icon });
              }}
            >
              {BLOCK_TYPES.map(t => (
                <option key={t.value} value={t.value}>{t.label}</option>
              ))}
            </select>
          </Field>

          <Field label="Icon">
            <IconPicker value={block.icon} onChange={(i) => updateBlock({ icon: i })} />
          </Field>

          <Field label="Überschrift">
            <input className="input" value={block.title} onChange={e => updateBlock({ title: e.target.value })} />
          </Field>

          <Field>
            <label className="row" style={{ cursor: 'pointer' }}>
              <input
                type="checkbox"
                checked={!!block.showTitle}
                onChange={e => updateBlock({ showTitle: e.target.checked })}
                style={{ accentColor: 'var(--accent)' }}
              />
              <span style={{ fontSize: 13 }}>Überschrift im Artikel anzeigen</span>
            </label>
          </Field>

          {block.type === 'bild' && (
            <>
              <Field label="Ausrichtung">
                <select className="select" value={block.align || 'center'} onChange={e => updateBlock({ align: e.target.value })}>
                  <option value="left">Links</option>
                  <option value="center">Zentriert</option>
                  <option value="right">Rechts</option>
                </select>
              </Field>
              <Field label="Max. Breite (px)">
                <input
                  type="number"
                  className="input mono"
                  value={block.maxWidth || 600}
                  onChange={e => updateBlock({ maxWidth: parseInt(e.target.value, 10) })}
                  style={{ width: 120 }}
                />
              </Field>
            </>
          )}
        </Panel>
      </div>

      <div className="stage-foot">
        <div className="muted" style={{ fontSize: 12 }}>
          <span className="kbd">→</span> oder Klick auf Schritt 1
        </div>
        <button className="btn primary" onClick={onNext}>
          Weiter zur Eingabe
          <Icon name="arrowRight" size={14} strokeWidth={2.4}/>
        </button>
      </div>
    </div>
  );
}

function ColorRow({ label, value, onChange }) {
  return (
    <div className="color-field" style={{ marginBottom: 10 }}>
      <label>{label}</label>
      <div className="color-swatch" style={{ background: value }}>
        <input type="color" value={value} onChange={e => onChange(e.target.value)} />
      </div>
      <input className="input mono" value={value} onChange={e => onChange(e.target.value)} />
    </div>
  );
}

Object.assign(window, { StageLayout, BLOCK_TYPES, typeLabel });

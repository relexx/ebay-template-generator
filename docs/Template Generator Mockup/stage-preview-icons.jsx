/* global React, generateHTML, Icon */
/* ============================================================
   Stage 2 — Vorschau (icons variant)
   ============================================================ */

function StagePreview({ template, content, onBack, onNext }) {
  const html = useMemo(() => generateHTML(template, content), [template, content]);
  const [device, setDevice] = useState('desktop');

  return (
    <div className="stage">
      <div className="page-head">
        <div>
          <h1>Vorschau · Live-Rendering</h1>
          <p className="page-sub">So sieht dein Artikel im eBay-Listing aus. Änderungen werden in Echtzeit übernommen.</p>
        </div>
        <div className="page-head-actions">
          <div className="btn-stack" role="tablist" aria-label="Geräteansicht">
            <button
              className={`btn sm${device === 'desktop' ? ' primary' : ''}`}
              onClick={() => setDevice('desktop')}
              aria-pressed={device === 'desktop'}
            >
              <Icon name="monitor" size={13}/>
              Desktop
            </button>
            <button
              className={`btn sm${device === 'mobile' ? ' primary' : ''}`}
              onClick={() => setDevice('mobile')}
              aria-pressed={device === 'mobile'}
            >
              <Icon name="smartphone" size={13}/>
              Mobile
            </button>
          </div>
        </div>
      </div>

      <div className="preview-shell">
        <div
          className="preview-frame"
          style={{
            maxWidth: device === 'mobile' ? 380 : 760,
            transition: 'max-width 380ms cubic-bezier(0.16, 1, 0.3, 1)'
          }}
        >
          <iframe
            srcDoc={`<!DOCTYPE html><html><head><style>body{margin:0;padding:24px;font-family:'Geist',-apple-system,system-ui,sans-serif;background:${template.colors.bg};}</style></head><body>${html}</body></html>`}
            style={{
              width: '100%',
              minHeight: 760,
              border: 'none',
              borderRadius: 4,
              background: template.colors.bg,
              display: 'block',
            }}
            title="eBay-Vorschau"
          />
        </div>
      </div>

      <div className="stage-foot">
        <button className="btn" onClick={onBack}>
          <Icon name="arrowLeft" size={14} strokeWidth={2.4}/>
          Zurück zur Eingabe
        </button>
        <button className="btn primary" onClick={onNext}>
          HTML generieren
          <Icon name="arrowRight" size={14} strokeWidth={2.4}/>
        </button>
      </div>
    </div>
  );
}

Object.assign(window, { StagePreview });

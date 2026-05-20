/* global React, generateHTML, syntaxHighlight, Icon */
/* ============================================================
   Stage 3 — HTML output (icons variant)
   ============================================================ */

function StageHtml({ template, content, onBack, onNewArticle, toast }) {
  const html = useMemo(() => generateHTML(template, content), [template, content]);
  const stats = useMemo(() => {
    const bytes = new Blob([html]).size;
    const lines = html.split('\n').length;
    return { bytes, lines };
  }, [html]);

  const copy = async () => {
    try {
      await navigator.clipboard.writeText(html);
      toast('HTML in Zwischenablage kopiert');
    } catch (e) {
      toast('Kopieren fehlgeschlagen');
    }
  };

  const download = () => {
    const blob = new Blob([html], { type: 'text/html' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `${template.name.toLowerCase().replace(/\s+/g, '-')}.html`;
    a.click();
    URL.revokeObjectURL(url);
    toast('HTML-Datei heruntergeladen');
  };

  return (
    <div className="stage">
      <div className="page-head">
        <div>
          <h1>HTML · Export</h1>
          <p className="page-sub">Fertig generiertes HTML — bereit zum Einfügen in deine eBay-Artikelbeschreibung.</p>
        </div>
        <div className="page-head-actions">
          <span className="tag mono">{stats.lines} Zeilen</span>
          <span className="tag mono">{(stats.bytes / 1024).toFixed(1)} kB</span>
          <span className="tag accent">
            <Icon name="check" size={10} strokeWidth={3}/> valid
          </span>
        </div>
      </div>

      <div className="toolbar">
        <button className="btn primary sm" onClick={copy}>
          <Icon name="copy" size={13}/>
          Kopieren
        </button>
        <button className="btn sm" onClick={download}>
          <Icon name="download" size={13}/>
          Herunterladen
        </button>
        <div className="divider" />
        <span className="muted mono" style={{ fontSize: 11 }}>UTF-8 · text/html</span>
        <div style={{ flex: 1 }} />
        <span className="muted" style={{ fontSize: 12 }}>Layout: <span className="mono">{template.name}</span></span>
      </div>

      <div className="code-block">
        {syntaxHighlight(html)}
      </div>

      <div className="stage-foot">
        <button className="btn" onClick={onBack}>
          <Icon name="arrowLeft" size={14} strokeWidth={2.4}/>
          Zurück zur Vorschau
        </button>
        <button className="btn primary" onClick={onNewArticle}>
          <Icon name="refresh" size={14} strokeWidth={2.2}/>
          Neuer Artikel
        </button>
      </div>
    </div>
  );
}

Object.assign(window, { StageHtml });

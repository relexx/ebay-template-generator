/* global React, ReactDOM, Wordmark, Stepper, ThemeToggle, Toast,
          StageLayout, StageInput, StagePreview, StageHtml,
          TweaksPanel, TweakSection, TweakColor, TweakRadio, useTweaks */
/* ============================================================
   relexx Template Generator — icons variant
   ============================================================ */

const TWEAK_DEFAULTS = /*EDITMODE-BEGIN*/{
  "accent": "#10b981",
  "theme": "dark",
  "density": "comfortable"
}/*EDITMODE-END*/;

const ACCENT_PRESETS = {
  '#10b981': { h: 162, c: 0.15, l: 0.65 },
  '#6366f1': { h: 270, c: 0.16, l: 0.62 },
  '#f97316': { h: 50,  c: 0.16, l: 0.68 },
  '#e11d48': { h: 15,  c: 0.18, l: 0.60 },
  '#f5c518': { h: 95,  c: 0.17, l: 0.82 },
};

const DEFAULT_TEMPLATE = {
  id: 'standard',
  name: 'Standard',
  colors: {
    primary: '#1a1a1a',
    accent: '#f5c518',
    bg: '#f8f9fa',
  },
  blocks: [
    { id: 'img',          title: 'Produktbild',      type: 'bild',             icon: 'image',    showTitle: false, align: 'center', maxWidth: 600 },
    { id: 'description',  title: 'Beschreibung',     type: 'fließtext',         icon: 'fileText', showTitle: true },
    { id: 'highlights',   title: 'Highlights',       type: 'stichwortkarten',   icon: 'sparkles', showTitle: true },
    { id: 'specs',        title: 'Technische Daten', type: 'datentabelle',     icon: 'settings', showTitle: true },
    { id: 'compat',       title: 'Kompatibilität',   type: 'featurekarten',    icon: 'wrench',   showTitle: true },
  ],
};

const DEFAULT_CONTENT = {
  title: 'Premium Produkt XYZ-3000',
  subtitle: 'Hochwertige Qualität | Art.-Nr. XYZ-3000-BK',
  summary: 'Das Premium Produkt XYZ-3000 überzeugt durch erstklassige Verarbeitung und innovative Features. Perfekt geeignet für anspruchsvolle Anwender, die Wert auf Qualität legen.',
  img: { url: 'https://placehold.co/600x400/1a1a1a/10b981?text=Produktbild' },
  description: { text: '**Das Produkt** bietet herausragende Qualität und durchdachte Features.\n\nDie hochwertige Verarbeitung garantiert *langlebige* Zuverlässigkeit für den täglichen Einsatz.' },
  highlights: { raw: 'Premium Qualität | Erstklassige Materialien und Verarbeitung\nInnovatives Design | Moderne Optik trifft Funktionalität\nEinfache Bedienung | Intuitive Handhabung für jeden\nLanglebigkeit | Robust und zuverlässig' },
  techData: { raw: 'Material | Aluminium / Kunststoff\nAbmessungen | 250 × 150 × 80 mm\nGewicht | 450 g\nFarbe | Schwarz\nAnschlüsse | USB-C, HDMI\nBetriebssystem | Windows / macOS / Linux' },
  compatibility: { raw: 'Windows 10/11 | Plug & Play, keine Treiber nötig\nmacOS 12+ | Vollständig kompatibel\nLinux | Standard-Treiber unterstützt' },
};

function App() {
  const [t, setTweak] = useTweaks(TWEAK_DEFAULTS);
  const [stage, setStage] = useState(0);
  const [maxReached, setMaxReached] = useState(0);
  const [toastMsg, setToastMsg] = useState(null);
  const [template, setTemplate] = useState(DEFAULT_TEMPLATE);
  const [content, setContent] = useState(DEFAULT_CONTENT);
  const [selectedBlockId, setSelectedBlockId] = useState(DEFAULT_TEMPLATE.blocks[0].id);

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', t.theme);
  }, [t.theme]);

  useEffect(() => {
    document.documentElement.setAttribute('data-density', t.density);
  }, [t.density]);

  useEffect(() => {
    const preset = ACCENT_PRESETS[t.accent];
    if (preset) {
      const root = document.documentElement.style;
      root.setProperty('--accent-h', preset.h);
      root.setProperty('--accent-c', preset.c);
      root.setProperty('--accent-l', preset.l);
    }
  }, [t.accent]);

  const goStage = useCallback((n) => {
    setStage(n);
    setMaxReached(prev => Math.max(prev, n));
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, []);

  const toast = useCallback((msg) => setToastMsg(msg), []);

  const updateTemplate = (patch) => setTemplate(prev => ({ ...prev, ...patch }));
  const updateContent = (patch) => setContent(prev => ({ ...prev, ...patch }));

  const onNewArticle = () => {
    setContent(DEFAULT_CONTENT);
    goStage(0);
    toast('Neuer Artikel angelegt');
  };

  useEffect(() => {
    const onKey = (e) => {
      if (e.target.matches('input, textarea, select, [contenteditable]')) return;
      if (e.key === 'ArrowRight' && stage < 3) goStage(stage + 1);
      if (e.key === 'ArrowLeft' && stage > 0) goStage(stage - 1);
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [stage, goStage]);

  return (
    <>
      <header className="topbar">
        <div className="topbar-inner">
          <div className="row">
            <Wordmark />
          </div>
          <div className="topbar-context" aria-label="Aktueller Artikel">
            <span className="product">{content.title}</span>
            <span className="sep" />
            <span className="layout-chip">{template.name.toLowerCase()}</span>
          </div>
          <div className="topbar-right">
            <Stepper stage={stage} setStage={goStage} maxReached={maxReached} variant="desktop" />
            <ThemeToggle theme={t.theme} setTheme={(v) => setTweak('theme', v)} />
          </div>
        </div>
      </header>

      <div className="stepper-mobile">
        <Stepper stage={stage} setStage={goStage} maxReached={maxReached} variant="mobile" />
      </div>

      <main className="shell">
        <div className="stage-area" data-screen-label={`0${stage + 1} ${['Layout','Eingabe','Vorschau','HTML'][stage]}`}>
          {stage === 0 && (
            <StageLayout
              key="s0"
              template={template}
              updateTemplate={updateTemplate}
              selectedBlockId={selectedBlockId}
              setSelectedBlockId={setSelectedBlockId}
              onNext={() => goStage(1)}
            />
          )}
          {stage === 1 && (
            <StageInput
              key="s1"
              template={template}
              content={content}
              updateContent={updateContent}
              onBack={() => goStage(0)}
              onNext={() => goStage(2)}
            />
          )}
          {stage === 2 && (
            <StagePreview
              key="s2"
              template={template}
              content={content}
              onBack={() => goStage(1)}
              onNext={() => goStage(3)}
            />
          )}
          {stage === 3 && (
            <StageHtml
              key="s3"
              template={template}
              content={content}
              onBack={() => goStage(2)}
              onNewArticle={onNewArticle}
              toast={toast}
            />
          )}
        </div>
      </main>

      <Toast message={toastMsg} onDone={() => setToastMsg(null)} />

      <TweaksPanel title="Tweaks">
        <TweakSection label="Theme" />
        <TweakRadio
          label="Modus"
          value={t.theme}
          options={['dark', 'light']}
          onChange={(v) => setTweak('theme', v)}
        />
        <TweakRadio
          label="Dichte"
          value={t.density}
          options={['comfortable', 'compact']}
          onChange={(v) => setTweak('density', v)}
        />
        <TweakSection label="Akzentfarbe" />
        <TweakColor
          label="Akzent"
          value={t.accent}
          options={['#10b981', '#6366f1', '#f97316', '#e11d48', '#f5c518']}
          onChange={(v) => setTweak('accent', v)}
        />
      </TweaksPanel>
    </>
  );
}

ReactDOM.createRoot(document.getElementById('root')).render(<App />);

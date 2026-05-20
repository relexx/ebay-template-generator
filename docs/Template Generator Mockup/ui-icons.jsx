/* global React, Icon */
/* ============================================================
   Shared UI primitives — icon variant
   ============================================================ */

const { useState, useEffect, useRef, useCallback, useMemo } = React;

/* ----- Wordmark ----- */
function Wordmark() {
  return (
    <div className="wordmark" aria-label="relexx Template Generator">
      <span className="wordmark-glyph">
        <svg viewBox="0 0 22 22" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect x="1" y="1" width="20" height="20" rx="2" stroke="currentColor" strokeOpacity="0.18" strokeWidth="1"/>
          <path d="M5 16 L5 6 L11 6 L11 11 L16 16" stroke="var(--accent)" strokeWidth="1.6" strokeLinecap="square" strokeLinejoin="miter"/>
          <rect x="14" y="4" width="3" height="3" fill="var(--accent)"/>
        </svg>
      </span>
      <span className="wordmark-text">
        <span className="accent">relexx</span><span>_tmpl</span><span className="cursor"></span>
      </span>
      <span className="wordmark-sub">Template Generator</span>
    </div>
  );
}

/* ----- Stepper ----- */
const STAGE_LABELS = ['Layout', 'Eingabe', 'Vorschau', 'HTML'];

function Stepper({ stage, setStage, maxReached, variant = 'desktop' }) {
  return (
    <div className={variant === 'mobile' ? 'stepper-mobile-track' : 'stepper'} role="tablist">
      {STAGE_LABELS.map((label, i) => {
        const isActive = i === stage;
        const isDone = i < stage || (maxReached >= i && i !== stage);
        return (
          <React.Fragment key={label}>
            <button
              className={`step ${isActive ? 'is-active' : ''} ${isDone && !isActive ? 'is-done' : ''}`}
              onClick={() => setStage(i)}
              role="tab"
              aria-selected={isActive}
              aria-current={isActive ? 'step' : undefined}
            >
              <span className="step-num">{i}</span>
              {label}
            </button>
            {i < STAGE_LABELS.length - 1 && variant === 'desktop' && (
              <span className={`step-connector ${i < stage ? 'is-done' : ''}`} aria-hidden="true" />
            )}
          </React.Fragment>
        );
      })}
    </div>
  );
}

/* ----- Theme toggle ----- */
function ThemeToggle({ theme, setTheme }) {
  const isDark = theme === 'dark';
  return (
    <button
      className="icon-btn"
      onClick={() => setTheme(isDark ? 'light' : 'dark')}
      aria-label={isDark ? 'Hellen Modus aktivieren' : 'Dunklen Modus aktivieren'}
      title={isDark ? 'Hellen Modus aktivieren' : 'Dunklen Modus aktivieren'}
    >
      <Icon name={isDark ? 'sun' : 'moon'} size={14} />
    </button>
  );
}

/* ----- Toast ----- */
function Toast({ message, onDone }) {
  useEffect(() => {
    if (!message) return;
    const t = setTimeout(onDone, 1900);
    return () => clearTimeout(t);
  }, [message, onDone]);
  if (!message) return null;
  return (
    <div className="toast" role="status">
      <Icon name="check" size={14} strokeWidth={2.4} />
      {message}
    </div>
  );
}

/* ----- Icon picker (now using line-icon name strings) ----- */
const BLOCK_ICON_NAMES = [
  'image', 'fileText', 'sparkles', 'settings', 'wrench',
  'box', 'lightbulb', 'star', 'target', 'barChart',
  'search', 'zap', 'hammer', 'ruler', 'palette', 'pin',
];

function IconPicker({ value, onChange }) {
  return (
    <div className="icon-picker">
      {BLOCK_ICON_NAMES.map(name => (
        <button
          key={name}
          type="button"
          className={`icon-tile ${value === name ? 'is-active' : ''}`}
          onClick={() => onChange(name)}
          aria-label={`Icon ${name}`}
          title={name}
        >
          <Icon name={name} size={18} strokeWidth={1.6} />
        </button>
      ))}
    </div>
  );
}

/* ----- Panel ----- */
function Panel({ title, icon, badge, children, actions, tight }) {
  return (
    <div className="panel">
      {title && (
        <div className="panel-head">
          <h2>
            {icon && (
              <span style={{ display: 'inline-grid', placeItems: 'center', width: 16, height: 16, color: 'var(--muted)' }}>
                <Icon name={icon} size={14} strokeWidth={1.75} />
              </span>
            )}
            {title}
            {badge && <span className="badge">{badge}</span>}
          </h2>
          {actions && <div className="row">{actions}</div>}
        </div>
      )}
      <div className={`panel-body${tight ? ' tight' : ''}`}>{children}</div>
    </div>
  );
}

/* ----- Field ----- */
function Field({ label, hint, children }) {
  return (
    <div className="field">
      {label && (
        <div className="field-label">
          <span>{label}</span>
          {hint && <span className="hint">{hint}</span>}
        </div>
      )}
      {children}
    </div>
  );
}

Object.assign(window, {
  Wordmark, Stepper, ThemeToggle, Toast, IconPicker, Panel, Field,
  STAGE_LABELS, BLOCK_ICON_NAMES,
});

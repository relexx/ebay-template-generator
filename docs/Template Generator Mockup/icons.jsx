/* global React */
/* ============================================================
   Icon pack — Lucide-style line icons
   ============================================================ */

const ICON_PATHS = {
  // Layout / chrome
  layers:    <g><path d="M12 2L2 7l10 5 10-5-10-5z"/><path d="M2 17l10 5 10-5"/><path d="M2 12l10 5 10-5"/></g>,
  grid:      <g><rect x="3" y="3" width="7" height="7" rx="1"/><rect x="14" y="3" width="7" height="7" rx="1"/><rect x="3" y="14" width="7" height="7" rx="1"/><rect x="14" y="14" width="7" height="7" rx="1"/></g>,
  blocks:    <g><rect x="3" y="3" width="7" height="7" rx="1"/><rect x="14" y="3" width="7" height="7" rx="1"/><rect x="14" y="14" width="7" height="7" rx="1"/><path d="M3 14h7v7H3z"/></g>,
  ruler:     <g><path d="M3 17l6-6 6 6 6-6"/><path d="M21 7v10H3V7"/></g>,

  // Stages
  edit:      <g><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></g>,
  trash:     <g><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/><path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/></g>,
  plus:      <g><path d="M12 5v14"/><path d="M5 12h14"/></g>,
  palette:   <g><circle cx="13.5" cy="6.5" r="1.5"/><circle cx="17.5" cy="10.5" r="1.5"/><circle cx="8.5" cy="7.5" r="1.5"/><circle cx="6.5" cy="12.5" r="1.5"/><path d="M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.554C21.965 6.012 17.461 2 12 2z"/></g>,
  monitor:   <g><rect x="2" y="3" width="20" height="14" rx="2"/><path d="M8 21h8"/><path d="M12 17v4"/></g>,
  smartphone:<g><rect x="6" y="2" width="12" height="20" rx="2"/><path d="M12 18h.01"/></g>,
  pin:       <g><path d="M12 2v10"/><path d="M9 12h6"/><path d="M9 12l3 10 3-10"/><circle cx="12" cy="6" r="3"/></g>,
  image:     <g><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="9" cy="9" r="2"/><path d="M21 15l-5-5L5 21"/></g>,
  fileText:  <g><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><path d="M14 2v6h6"/><path d="M8 13h8"/><path d="M8 17h6"/></g>,
  sparkles:  <g><path d="M12 3l1.8 4.8L18 9.6l-4.2 1.8L12 16.2 10.2 11.4 6 9.6l4.2-1.8z"/><path d="M19 14l.8 2.2L22 17l-2.2.8L19 20l-.8-2.2L16 17l2.2-.8z"/></g>,
  settings:  <g><circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/></g>,
  wrench:    <g><path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"/></g>,
  box:       <g><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><path d="M3.27 6.96L12 12.01l8.73-5.05"/><path d="M12 22V12"/></g>,
  lightbulb: <g><path d="M9 18h6"/><path d="M10 22h4"/><path d="M12 2a7 7 0 0 0-4 12.7c.5.5 1 1.3 1 2.3h6c0-1 .5-1.8 1-2.3A7 7 0 0 0 12 2z"/></g>,
  star:      <g><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77 5.82 21l1.18-6.88-5-4.87 6.91-1.01z"/></g>,
  target:    <g><circle cx="12" cy="12" r="10"/><circle cx="12" cy="12" r="6"/><circle cx="12" cy="12" r="2"/></g>,
  barChart:  <g><path d="M3 3v18h18"/><rect x="7" y="13" width="3" height="6"/><rect x="12" y="9" width="3" height="10"/><rect x="17" y="5" width="3" height="14"/></g>,
  search:    <g><circle cx="11" cy="11" r="7"/><path d="M21 21l-4.35-4.35"/></g>,
  zap:       <g><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></g>,
  hammer:    <g><path d="M15 12l-8.5 8.5a2.12 2.12 0 0 1-3-3L12 9"/><path d="M17.6 6.4l4 4M11.8 6.4l-5 5"/></g>,

  // Actions / nav
  arrowRight:<g><path d="M5 12h14"/><path d="M13 5l7 7-7 7"/></g>,
  arrowLeft: <g><path d="M19 12H5"/><path d="M11 19l-7-7 7-7"/></g>,
  refresh:   <g><path d="M21 12a9 9 0 1 1-3-6.7L21 8"/><path d="M21 3v5h-5"/></g>,
  copy:      <g><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></g>,
  download:  <g><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><path d="M7 10l5 5 5-5"/><path d="M12 15V3"/></g>,
  upload:    <g><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><path d="M17 8l-5-5-5 5"/><path d="M12 3v12"/></g>,
  fileJson:  <g><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><path d="M14 2v6h6"/><path d="M10 13a1 1 0 0 0-1 1v2a1 1 0 0 1-1 1 1 1 0 0 1 1 1v2a1 1 0 0 0 1 1"/><path d="M14 13a1 1 0 0 1 1 1v2a1 1 0 0 0 1 1 1 1 0 0 0-1 1v2a1 1 0 0 1-1 1"/></g>,
  camera:    <g><path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/><circle cx="12" cy="13" r="4"/></g>,
  check:     <g><path d="M20 6L9 17l-5-5"/></g>,
  gripVert:  <g><circle cx="9" cy="6" r="1"/><circle cx="9" cy="12" r="1"/><circle cx="9" cy="18" r="1"/><circle cx="15" cy="6" r="1"/><circle cx="15" cy="12" r="1"/><circle cx="15" cy="18" r="1"/></g>,
  sun:       <g><circle cx="12" cy="12" r="4"/><path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M4.93 19.07l1.41-1.41M17.66 6.34l1.41-1.41"/></g>,
  moon:      <g><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/></g>,
  type:      <g><path d="M4 7V4h16v3"/><path d="M9 20h6"/><path d="M12 4v16"/></g>,
  code:      <g><path d="M16 18l6-6-6-6"/><path d="M8 6l-6 6 6 6"/></g>,
  eye:       <g><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></g>,
  layoutPanel: <g><rect x="3" y="3" width="18" height="18" rx="2"/><path d="M3 9h18"/><path d="M9 21V9"/></g>,
};

function Icon({ name, size = 14, strokeWidth = 1.75, style, className }) {
  const path = ICON_PATHS[name];
  if (!path) return null;
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth={strokeWidth}
      strokeLinecap="round"
      strokeLinejoin="round"
      style={style}
      className={className}
      aria-hidden="true"
    >
      {path}
    </svg>
  );
}

Object.assign(window, { Icon, ICON_PATHS });

import React from 'react';

export default function VistaPrevia({ producto, lineaSeleccionada, valores, t }) {
    const colorHex = lineaSeleccionada?.colorPrimario || '#cccccc';

    return (
        <div style={{
            marginBottom: 12, position: "relative", borderRadius: 16, overflow: "hidden",
            background: "#f4f4f4", userSelect: "none", border: `1px solid ${t.bd2}`, aspectRatio: "1 / 1"
        }}>
            {producto?.imagenUrl ? (
                <img
                    src={producto.imagenUrl}
                    alt={producto.nombre}
                    style={{ width: "100%", height: "100%", display: "block", objectFit: "contain", pointerEvents: "none" }}
                />
            ) : (
                <div style={{ 
                    width: "100%", height: "100%", display: "flex", 
                    alignItems: "center", justifyItems: "center", color: t.tx3 
                }}>
                    <span style={{ margin: "auto" }}>Sin vista previa</span>
                </div>
            )}

            {/* Overlay de color */}
            <div style={{
                position: "absolute", inset: 0, backgroundColor: colorHex, mixBlendMode: "hue",
                opacity: 0.85, transition: "background-color 0.3s", pointerEvents: "none",
            }} />

            {/* Personalización visual simplificada (solo número) */}
            {valores.numeroEconomico && (
                <div style={{
                    position: "absolute", left: '50%', top: '75%',
                    transform: "translate(-50%, -50%)",
                    padding: "4px 10px", background: "rgba(0,0,0,0.65)", color: "#fff",
                    fontWeight: 700, fontFamily: t.ffH, fontSize: 17, borderRadius: 5, letterSpacing: 2,
                    boxShadow: "0 2px 10px rgba(0,0,0,0.5)", border: "1px dashed rgba(255,255,255,0.35)", whiteSpace: "nowrap",
                }}>
                    {valores.numeroEconomico}
                </div>
            )}
            
            {lineaSeleccionada && (
                <div style={{
                    position: "absolute", bottom: 20, right: 20,
                    background: "rgba(255,255,255,0.9)", padding: "5px 10px", borderRadius: 5,
                    display: "flex", alignItems: "center", gap: 8, boxShadow: "0 2px 5px rgba(0,0,0,0.2)"
                }}>
                    {lineaSeleccionada.logoUrl && (
                        <img src={lineaSeleccionada.logoUrl} alt="Logo" style={{ height: 20 }} />
                    )}
                    <span style={{ fontSize: 10, fontWeight: 700, color: '#000' }}>{lineaSeleccionada.nombre}</span>
                </div>
            )}
        </div>
    );
}

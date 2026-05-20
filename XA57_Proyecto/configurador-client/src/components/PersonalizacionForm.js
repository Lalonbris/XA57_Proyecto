import React from 'react';

export default function PersonalizacionForm({ tipoProducto, valores, onChange, errores, t }) {
    if (!tipoProducto) return null;

    const max = tipoProducto.maxCaracteres || 20;

    return (
        <div style={{ display: "flex", flexDirection: "column", gap: 15 }}>
            {tipoProducto.permiteNombre && (
                <div>
                    <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>
                        Nombre del Operador
                    </label>
                    <input 
                        type="text" 
                        maxLength={max} 
                        value={valores.nombreOperador || ''} 
                        onChange={(e) => onChange('nombreOperador', e.target.value)}
                        placeholder="Ej. Juan Pérez"
                        style={{ 
                            width: "100%", padding: "9px 12px", border: `1px solid ${errores.nombreOperador ? '#ef4444' : t.bd2}`, 
                            borderRadius: 8, fontSize: 13, color: t.tx0, background: t.bg3, outline: "none" 
                        }}
                    />
                    <div style={{ display: "flex", justifyContent: "space-between", marginTop: 4 }}>
                        <span style={{ color: '#ef4444', fontSize: 11 }}>{errores.nombreOperador}</span>
                        <span style={{ fontSize: 10, color: (valores.nombreOperador?.length || 0) > max ? '#ef4444' : t.tx3 }}>
                            {valores.nombreOperador?.length || 0}/{max}
                        </span>
                    </div>
                </div>
            )}

            {tipoProducto.permiteNumeroEconomico && (
                <div>
                    <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>
                        Número Económico
                    </label>
                    <input 
                        type="text" 
                        maxLength={max} 
                        value={valores.numeroEconomico || ''} 
                        onChange={(e) => onChange('numeroEconomico', e.target.value)}
                        placeholder="Ej. 105"
                        style={{ 
                            width: "100%", padding: "9px 12px", border: `1px solid ${errores.numeroEconomico ? '#ef4444' : t.bd2}`, 
                            borderRadius: 8, fontSize: 13, color: t.tx0, background: t.bg3, outline: "none" 
                        }}
                    />
                    <div style={{ display: "flex", justifyContent: "space-between", marginTop: 4 }}>
                        <span style={{ color: '#ef4444', fontSize: 11 }}>{errores.numeroEconomico}</span>
                        <span style={{ fontSize: 10, color: (valores.numeroEconomico?.length || 0) > max ? '#ef4444' : t.tx3 }}>
                            {valores.numeroEconomico?.length || 0}/{max}
                        </span>
                    </div>
                </div>
            )}

            {tipoProducto.permiteRuta && (
                <div>
                    <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>
                        Ruta / Destino
                    </label>
                    <input 
                        type="text" 
                        maxLength={max} 
                        value={valores.ruta || ''} 
                        onChange={(e) => onChange('ruta', e.target.value)}
                        placeholder="Ej. México - Guadalajara"
                        style={{ 
                            width: "100%", padding: "9px 12px", border: `1px solid ${errores.ruta ? '#ef4444' : t.bd2}`, 
                            borderRadius: 8, fontSize: 13, color: t.tx0, background: t.bg3, outline: "none" 
                        }}
                    />
                    <div style={{ display: "flex", justifyContent: "space-between", marginTop: 4 }}>
                        <span style={{ color: '#ef4444', fontSize: 11 }}>{errores.ruta}</span>
                        <span style={{ fontSize: 10, color: (valores.ruta?.length || 0) > max ? '#ef4444' : t.tx3 }}>
                            {valores.ruta?.length || 0}/{max}
                        </span>
                    </div>
                </div>
            )}

            <div>
                <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>
                    Notas Especiales (Opcional)
                </label>
                <textarea 
                    value={valores.notasEspeciales || ''} 
                    onChange={(e) => onChange('notasEspeciales', e.target.value)} 
                    rows={2}
                    placeholder="Instrucciones adicionales..."
                    style={{ 
                        width: "100%", padding: "9px 12px", border: `1px solid ${errores.notasEspeciales ? '#ef4444' : t.bd2}`, 
                        borderRadius: 8, fontSize: 13, resize: "none", color: t.tx0, background: t.bg3, outline: "none" 
                    }}
                />
                {errores.notasEspeciales && <p style={{ color: '#ef4444', fontSize: 11, marginTop: 4 }}>{errores.notasEspeciales}</p>}
            </div>
        </div>
    );
}

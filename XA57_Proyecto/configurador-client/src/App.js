import { useState } from "react";
import { useConfiguradorData } from "./hooks/useConfiguradorData";
import { validarConfiguracion } from "./utils/validaciones";
import ModeloSelector from "./components/ModeloSelector";
import LineaSelector from "./components/LineaSelector";
import PersonalizacionForm from "./components/PersonalizacionForm";
import VistaPrevia from "./components/VistaPrevia";
import CantidadControl from "./components/CantidadControl";

/* ── Design tokens ── */
const t = {
    bg0:  "#070707",
    bg1:  "#0f0f0f",
    bg2:  "#161616",
    bg3:  "#1d1d1d",
    bg4:  "#252525",
    bg5:  "#2e2e2e",
    bd1:  "#232323",
    bd2:  "#313131",
    bd3:  "#424242",
    tx0:  "#f5f5f5",
    tx1:  "#c4c4c4",
    tx2:  "#878787",
    tx3:  "#525252",
    sv1:  "#d8d8d8",
    white:"#ffffff",
    ffH:  "'Space Grotesk', system-ui, sans-serif",
    ffB:  "'Inter', system-ui, sans-serif",
};

export default function Configurador(props) {
    const { productoId } = props;

    // Fetch data from API
    const { modelos, lineas, producto, loading, error } = useConfiguradorData(productoId);

    // Form state
    const [config, setConfig] = useState({
        modeloAutobusId: null,
        lineaId: null,
        lineaSeleccionada: null,
        nombreOperador: "",
        numeroEconomico: "",
        ruta: "",
        notasEspeciales: "",
        cantidad: 1
    });

    const [errores, setErrores] = useState({});
    const [btnHover, setBtnHover] = useState(false);
    const [enviando, setEnviando] = useState(false);

    const handleUpdate = (field, value) => {
        setConfig(prev => ({ ...prev, [field]: value }));
        // Limpiar error del campo al escribir
        if (errores[field]) {
            setErrores(prev => {
                const newErr = { ...prev };
                delete newErr[field];
                return newErr;
            });
        }
    };

    const handleLineaSelect = (linea) => {
        setConfig(prev => ({ 
            ...prev, 
            lineaId: linea.id, 
            lineaSeleccionada: linea 
        }));
        if (errores.linea) {
            setErrores(prev => {
                const newErr = { ...prev };
                delete newErr.linea;
                return newErr;
            });
        }
    };

    const handleAgregarCarrito = async () => {
        const errs = validarConfiguracion(config, producto?.tipoProducto);
        if (Object.keys(errs).length > 0) {
            setErrores(errs);
            mostrarToast("Por favor corrija los errores antes de continuar.", true);
            return;
        }

        setEnviando(true);
        try {
            const payload = {
                productoId: parseInt(productoId),
                modeloAutobusId: config.modeloAutobusId,
                lineaId: config.lineaId,
                color: config.lineaSeleccionada?.nombre,
                colorHex: config.lineaSeleccionada?.colorPrimario,
                nombreOperador: config.nombreOperador || null,
                numeroEconomico: config.numeroEconomico || null,
                ruta: config.ruta || null,
                notasEspeciales: config.notasEspeciales || null,
                cantidad: config.cantidad
            };

            const response = await fetch("/Carrito/Agregar", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (response.ok) {
                mostrarToast("Producto agregado al carrito.");
                if (window.actualizarIconoCarrito) {
                    window.actualizarIconoCarrito();
                }
            } else {
                const data = await response.json();
                throw new Error(data.message || 'Error al agregar al carrito');
            }
        } catch (err) {
            mostrarToast(err.message, true);
        } finally {
            setEnviando(false);
        }
    };

    if (loading) return (
        <div style={{ padding: 40, textAlign: "center", width: "100%", color: t.tx2 }}>
            <span className="material-icons animate-spin" style={{ fontSize: 40, marginBottom: 10 }}>refresh</span>
            <p>Cargando configurador...</p>
        </div>
    );

    if (error) return (
        <div style={{ padding: 40, textAlign: "center", width: "100%", color: "#ef4444" }}>
            <span className="material-icons" style={{ fontSize: 40, marginBottom: 10 }}>error</span>
            <p>{error}</p>
            <button onClick={() => window.location.reload()} style={{ marginTop: 15, padding: "8px 16px", borderRadius: 8, background: t.bg3, color: t.tx0, border: `1px solid ${t.bd2}` }}>
                Reintentar
            </button>
        </div>
    );

    return (
        <div style={{ 
            fontFamily: t.ffB, color: t.tx0, width: "100%", 
            display: "grid", gridTemplateColumns: "1fr 1fr", gap: "40px", alignItems: "start"
        }}>

            {/* ── COLUMNA IZQUIERDA: Preview ── */}
            <VistaPrevia 
                producto={producto} 
                lineaSeleccionada={config.lineaSeleccionada} 
                valores={config} 
                t={t} 
            />

            {/* ── COLUMNA DERECHA: Controles ── */}
            <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
                <div style={{ marginBottom: 10 }}>
                    <h2 style={{ fontSize: 24, fontWeight: 700, fontFamily: t.ffH, color: t.tx0, margin: 0 }}>
                        {producto?.nombre}
                    </h2>
                    <p style={{ fontSize: 18, color: t.sv1, marginTop: 4, fontWeight: 600 }}>
                        ${producto?.precio?.toLocaleString('en-US', { minimumFractionDigits: 2 })}
                    </p>
                </div>

                <ModeloSelector 
                    modelos={modelos} 
                    selectedId={config.modeloAutobusId} 
                    onSelect={(id) => handleUpdate('modeloAutobusId', id)} 
                    error={errores.modelo}
                    t={t} 
                />

                <LineaSelector 
                    lineas={lineas} 
                    selectedLinea={config.lineaSeleccionada} 
                    onSelect={handleLineaSelect} 
                    error={errores.linea}
                    t={t} 
                />

                <div style={{ padding: "15px 0", borderTop: `1px solid ${t.bd1}`, borderBottom: `1px solid ${t.bd1}` }}>
                    <p style={{ fontWeight: 600, fontSize: 14, fontFamily: t.ffH, color: t.tx0, marginBottom: 15 }}>
                        Personalización del Producto
                    </p>
                    <PersonalizacionForm 
                        tipoProducto={producto?.tipoProducto} 
                        valores={config} 
                        onChange={handleUpdate} 
                        errores={errores}
                        t={t} 
                    />
                </div>

                {/* CANTIDAD + CARRITO */}
                <div style={{ display: "flex", gap: 10, marginTop: 10 }}>
                    <CantidadControl 
                        cantidad={config.cantidad} 
                        onChange={(val) => handleUpdate('cantidad', val)} 
                        t={t} 
                    />
                    <button
                        onClick={handleAgregarCarrito}
                        disabled={enviando}
                        onMouseEnter={() => setBtnHover(true)}
                        onMouseLeave={() => setBtnHover(false)}
                        style={{
                            flex: 1, padding: "13px", 
                            background: enviando ? t.bg4 : (btnHover ? t.sv1 : t.white), 
                            color: t.bg0, border: "none", borderRadius: 10,
                            fontSize: 14, fontWeight: 600, cursor: enviando ? "not-allowed" : "pointer", 
                            transition: "all 0.2s", 
                            boxShadow: btnHover && !enviando ? "0 4px 15px rgba(0,0,0,0.4)" : "none"
                        }}
                    >
                        {enviando ? "Agregando..." : "Agregar al carrito"}
                    </button>
                </div>
            </div>
            
            <style>{`
                @keyframes spin {
                    from { transform: rotate(0deg); }
                    to { transform: rotate(360deg); }
                }
                .animate-spin {
                    animation: spin 1s linear infinite;
                    display: inline-block;
                }
            `}</style>
        </div>
    );
}

/* ── Toast notification ── */
function mostrarToast(texto, esError = false) {
    const el = document.createElement("div");
    el.textContent = texto;
    Object.assign(el.style, {
        position: "fixed", bottom: "28px", right: "28px", 
        background: esError ? "#ef4444" : "#fff", 
        color: esError ? "#fff" : "#080808",
        padding: "13px 20px", borderRadius: "10px", fontFamily: "Inter, sans-serif", fontSize: "13px",
        fontWeight: "600", boxShadow: "0 8px 32px rgba(0,0,0,0.45)", zIndex: "9999", opacity: "0",
        transition: "opacity 0.2s ease, transform 0.2s ease", transform: "translateY(8px)",
    });
    document.body.appendChild(el);
    requestAnimationFrame(() => { el.style.opacity = "1"; el.style.transform = "translateY(0)"; });
    setTimeout(() => {
        el.style.opacity = "0"; el.style.transform = "translateY(8px)";
        setTimeout(() => el.remove(), 220);
    }, 3500);
}

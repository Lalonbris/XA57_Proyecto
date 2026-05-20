import { useState, useEffect } from "react";
import { useConfiguradorData } from "./hooks/useConfiguradorData";
import { validarConfiguracion } from "./utils/validaciones";
import ModeloSelector from "./components/ModeloSelector";
import LineaSelector from "./components/LineaSelector";
import PersonalizacionForm from "./components/PersonalizacionForm";
import VistaPrevia from "./components/VistaPrevia";
import CantidadControl from "./components/CantidadControl";
import AgregarCarritoBtn from "./components/AgregarCarritoBtn";

const t = {
    bg0:  "#070707", bg1:  "#0f0f0f", bg2:  "#161616",
    bg3:  "#1d1d1d", bg4:  "#252525", bg5:  "#2e2e2e",
    bd1:  "#232323", bd2:  "#313131", bd3:  "#424242",
    tx0:  "#f5f5f5", tx1:  "#c4c4c4", tx2:  "#878787",
    tx3:  "#525252", sv1:  "#d8d8d8", white:"#ffffff",
    ffH:  "'Space Grotesk', system-ui, sans-serif",
    ffB:  "'Inter', system-ui, sans-serif",
};

export default function App({ datasetProps }) {
    const { productoId } = datasetProps;
    const { modelos, lineas, producto, loading, error } = useConfiguradorData(productoId);

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

    const handleUpdate = (field, value) => {
        setConfig(prev => ({ ...prev, [field]: value }));
        if (errores[field]) {
            setErrores(prev => {
                const newErrors = { ...prev };
                delete newErrors[field];
                return newErrors;
            });
        }
    };

    const handleLineaSelect = (linea) => {
        handleUpdate('lineaId', linea.id);
        handleUpdate('lineaSeleccionada', linea);
    };

    const handleValidationFailed = (errs) => {
        setErrores(errs);
    };

    const getPayload = () => ({
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
    });

    if (loading) return (
        <div style={{ padding: 40, textAlign: "center", width: "100%", color: t.tx2, background: t.bg2, borderRadius: "20px" }}>
            <p>Cargando configurador...</p>
        </div>
    );

    if (error) return (
        <div style={{ padding: 40, textAlign: "center", width: "100%", color: "#ef4444", background: t.bg2, borderRadius: "20px" }}>
            <p>Error: {error}</p>
            <button onClick={() => window.location.reload()}>Reintentar</button>
        </div>
    );
        
    // ESTA ES LA CLAVE: No renderizar nada hasta que 'producto' exista.
    if (!producto) {
        return (
            <div style={{ padding: 40, textAlign: "center", width: "100%", color: t.tx2, background: t.bg2, borderRadius: "20px" }}>
                <p>Inicializando producto...</p>
            </div>
        );
    }
        
    const modeloSeleccionado = modelos.find(m => m.id === config.modeloAutobusId);

    return (
        <div style={{ 
            fontFamily: t.ffB, color: t.tx0, width: "100%", background: t.bg2, padding: "40px", borderRadius: "20px",
            display: "grid", gridTemplateColumns: "1fr 1fr", gap: "40px", alignItems: "start"
        }}>
            <VistaPrevia 
                producto={producto} 
                lineaSeleccionada={config.lineaSeleccionada} 
                personalizacion={config}
                modeloSeleccionado={modeloSeleccionado}
                t={t}
            />

            <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
                <div>
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

                <div style={{ display: "flex", alignItems: "flex-end", gap: 20, marginTop: 10 }}>
                    <CantidadControl 
                        cantidad={config.cantidad} 
                        setCantidad={(val) => handleUpdate('cantidad', val)}
                        t={t} 
                    />
                    <AgregarCarritoBtn 
                        configuracion={getPayload()}
                        tipoProducto={producto?.tipoProducto}
                        onValidationFailed={handleValidationFailed}
                        t={t}
                    />
                </div>
            </div>
        </div>
    );
}

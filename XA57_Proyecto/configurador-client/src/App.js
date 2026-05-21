import { useState, useRef } from "react";

const colores = [
    { nombre: "ADO Red",       hex: "#E8322A" },
    { nombre: "ETN Gold",     hex: "#b8860b" },
    { nombre: "Primera Plus", hex: "#F0C200" },
    { nombre: "Futura",       hex: "#a855f7" },
];

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
    const { 
        productoId, 
        productoImagen, 
        productoNombre = "Configurador XA57",
        permiteNumeroEconomico = "true",
        maxCaracteres = 20
    } = props;

    const [colorHex,        setColorHex]        = useState("#F0C200");
    const [colorNombre,     setColorNombre]      = useState("Primera Plus");
    const [numeroSerie,     setNumeroSerie]      = useState("");
    const [notasEspeciales, setNotasEspeciales]  = useState("");
    const [habilitado,      setHabilitado]       = useState(true);
    const [cantidad,        setCantidad]         = useState(1);
    
    // Dragging state
    const [textPos,         setTextPos]          = useState({ x: 50, y: 75 });
    const [imagePos,        setImagePos]         = useState({ x: 30, y: 40 });
    const [uploadedImage,   setUploadedImage]    = useState(null);
    const [dragItem,        setDragItem]         = useState(null); // 'text' | 'image'
    const [dragOffset,      setDragOffset]       = useState({ x: 0, y: 0 });
    const [btnHover,        setBtnHover]         = useState(false);
    
    const previewRef = useRef(null);

    const seleccionarColor = (color) => {
        setColorHex(color.hex);
        setColorNombre(color.nombre);
    };

    const handleColorLibre = (hex) => {
        setColorHex(hex);
        setColorNombre(`Custom: ${hex.toUpperCase()}`);
    };

    const handleFileUpload = (e) => {
        const file = e.target.files[0];
        if (file) {
            setUploadedImage(URL.createObjectURL(file));
        }
    };

    const handleAgregarCarrito = async () => {
        const datos = { productoId, color: colorNombre, colorHex, numeroSerie, notasEspeciales, cantidad };
        const response = await fetch("/Carrito/Agregar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(datos),
        });
        if (response.ok) {
            mostrarToast("Producto agregado al carrito.");
            if (window.actualizarIconoCarrito) {
                window.actualizarIconoCarrito();
            }
        }
    };

    /* ── Drag handlers ── */
    const startDrag = (e, item) => {
        e.preventDefault();
        const clientX = e.clientX || e.touches[0].clientX;
        const clientY = e.clientY || e.touches[0].clientY;
        
        setDragItem(item);
        const rect = previewRef.current.getBoundingClientRect();
        const pos = item === 'text' ? textPos : imagePos;
        
        setDragOffset({
            x: clientX - rect.left - (pos.x / 100) * rect.width,
            y: clientY - rect.top  - (pos.y / 100) * rect.height,
        });
    };

    const onMove = (e) => {
        if (!dragItem) return;
        const clientX = (e.clientX !== undefined) ? e.clientX : e.touches[0].clientX;
        const clientY = (e.clientY !== undefined) ? e.clientY : e.touches[0].clientY;
        
        const rect = previewRef.current.getBoundingClientRect();
        const newPos = {
            x: Math.min(Math.max(((clientX - rect.left - dragOffset.x) / rect.width)  * 100, 0), 95),
            y: Math.min(Math.max(((clientY - rect.top  - dragOffset.y) / rect.height) * 100, 0), 95),
        };
        
        if (dragItem === 'text') setTextPos(newPos);
        else if (dragItem === 'image') setImagePos(newPos);
    };

    const stopDrag = () => setDragItem(null);

    const permits = (val) => val === "true" || val === true;

    return (
        <div style={{ 
            fontFamily: t.ffB, color: t.tx0, width: "100%", 
            display: "grid", gridTemplateColumns: "1fr 1fr", gap: "40px", alignItems: "start"
        }}>

            {/* ── COLUMNA IZQUIERDA: Preview ── */}
            <div>
                <div
                    ref={previewRef}
                    onMouseMove={onMove}
                    onMouseUp={stopDrag}
                    onMouseLeave={stopDrag}
                    onTouchMove={onMove}
                    onTouchEnd={stopDrag}
                    style={{
                        marginBottom: 12, position: "relative", borderRadius: 16, overflow: "hidden",
                        background: "#f4f4f4", userSelect: "none", border: `1px solid ${t.bd2}`, aspectRatio: "1 / 1"
                    }}
                >
                    <img
                        src={productoImagen}
                        alt="Producto"
                        style={{ width: "100%", height: "100%", display: "block", objectFit: "contain", pointerEvents: "none" }}
                    />

                    <div style={{
                        position: "absolute", inset: 0, backgroundColor: colorHex, mixBlendMode: "hue",
                        opacity: 0.85, transition: "background-color 0.3s", pointerEvents: "none",
                    }} />

                    {/* Logo subido arrastrable */}
                    {uploadedImage && (
                        <div
                            onMouseDown={(e) => startDrag(e, 'image')}
                            onTouchStart={(e) => startDrag(e, 'image')}
                            style={{
                                position: "absolute", left: `${imagePos.x}%`, top: `${imagePos.y}%`,
                                transform: "translate(-50%, -50%)", cursor: dragItem === 'image' ? "grabbing" : "grab",
                                width: "60px", height: "60px"
                            }}
                        >
                            <img src={uploadedImage} alt="Custom Logo" style={{ width: "100%", height: "100%", objectFit: "contain", pointerEvents: "none" }} />
                        </div>
                    )}

                    {/* Numero arrastrable */}
                    {numeroSerie && permits(permiteNumeroEconomico) && (
                        <div
                            onMouseDown={(e) => startDrag(e, 'text')}
                            onTouchStart={(e) => startDrag(e, 'text')}
                            style={{
                                position: "absolute", left: `${textPos.x}%`, top: `${textPos.y}%`,
                                transform: "translate(-50%, -50%)", cursor: dragItem === 'text' ? "grabbing" : "grab",
                                padding: "4px 10px", background: "rgba(0,0,0,0.65)", color: "#fff",
                                fontWeight: 700, fontFamily: t.ffH, fontSize: 17, borderRadius: 5, letterSpacing: 2,
                                boxShadow: "0 2px 10px rgba(0,0,0,0.5)", border: "1px dashed rgba(255,255,255,0.35)", whiteSpace: "nowrap",
                            }}
                        >
                            {numeroSerie}
                        </div>
                    )}
                </div>
                {(numeroSerie || uploadedImage) && (
                    <p style={{ textAlign: "center", fontSize: 11, color: t.tx3 }}>
                        Arrastra los elementos sobre el preview para acomodarlos
                    </p>
                )}
            </div>

            {/* ── COLUMNA DERECHA: Controles ── */}
            <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
                <div style={{ marginBottom: 10 }}>
                    <h2 style={{ fontSize: 24, fontWeight: 700, fontFamily: t.ffH, color: t.tx0, margin: 0 }}>{productoNombre}</h2>
                    <p style={{ fontSize: 14, color: t.tx2, marginTop: 4 }}>{colorNombre}</p>
                </div>

                {/* TOGGLE */}
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", paddingBottom: 15, borderBottom: `1px solid ${t.bd1}` }}>
                    <div style={{ display: "flex", flexDirection: "column", gap: 2 }}>
                        <span style={{ fontWeight: 600, fontSize: 14, fontFamily: t.ffH, color: t.tx0 }}>Personalize Your Item</span>
                        <span style={{ fontSize: 10, fontWeight: 600, letterSpacing: 2, textTransform: "uppercase", color: t.tx3 }}>Studio</span>
                    </div>
                    <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                        <div onClick={() => setHabilitado(!habilitado)} style={{
                            width: 42, height: 22, borderRadius: 11, cursor: "pointer",
                            background: habilitado ? t.sv1 : t.bg5, position: "relative",
                            transition: "background 0.2s", border: `1px solid ${habilitado ? t.sv1 : t.bd2}`,
                        }}>
                            <div style={{
                                width: 16, height: 16, borderRadius: "50%", background: habilitado ? t.bg0 : t.tx3,
                                position: "absolute", top: 2, left: habilitado ? 22 : 2, transition: "left 0.2s"
                            }} />
                        </div>
                    </div>
                </div>

                {habilitado && (
                    <>
                        {/* Cargar Imagen */}
                        <div>
                            <p style={{ fontWeight: 600, fontSize: 12, marginBottom: 10, color: t.tx1, fontFamily: t.ffH }}>Subir Logotipo / Imagen</p>
                            <label style={{
                                display: "block", width: "100%", padding: "12px", background: t.bg3, border: `1px dashed ${t.bd2}`,
                                borderRadius: 8, cursor: "pointer", textAlign: "center", color: t.tx2, fontSize: 12
                            }}>
                                <span className="material-icons" style={{ verticalAlign: "middle", marginRight: 8, fontSize: 18 }}>cloud_upload</span>
                                {uploadedImage ? "Cambiar Imagen" : "Seleccionar archivo"}
                                <input type="file" accept="image/*" onChange={handleFileUpload} style={{ display: "none" }} />
                            </label>
                        </div>

                        {/* Colores */}
                        <div>
                            <p style={{ fontWeight: 600, fontSize: 12, marginBottom: 10, color: t.tx1, fontFamily: t.ffH }}>Esquema de Color</p>
                            <div style={{ display: "flex", gap: 10, flexWrap: "wrap" }}>
                                {colores.map((color) => (
                                    <div key={color.nombre} onClick={() => seleccionarColor(color)} style={{
                                        cursor: "pointer", border: colorNombre === color.nombre ? `2px solid ${t.sv1}` : "2px solid transparent",
                                        borderRadius: 10, padding: 3
                                    }}>
                                        <div style={{ width: 60, height: 32, borderRadius: 7, background: color.hex }} />
                                    </div>
                                ))}
                                <div style={{ position: "relative", width: 60, height: 32 }}>
                                    <div style={{ width: 60, height: 32, borderRadius: 7, background: "linear-gradient(135deg, #ff0000, #ffff00, #0000ff)" }} />
                                    <input type="color" value={colorHex} onChange={(e) => handleColorLibre(e.target.value)} style={{ position: "absolute", inset: 0, width: "100%", height: "100%", opacity: 0, cursor: "pointer" }} />
                                </div>
                            </div>
                        </div>

                        {/* Numero Serie */}
                        {permits(permiteNumeroEconomico) && (
                            <div>
                                <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>Numero de Unidad</label>
                                <div style={{ position: "relative" }}>
                                    <span style={{ position: "absolute", left: 12, top: "50%", transform: "translateY(-50%)", color: t.tx3, fontSize: 13 }}>#</span>
                                    <input type="text" placeholder="ej. 9582" maxLength={maxCaracteres} value={numeroSerie} onChange={(e) => setNumeroSerie(e.target.value)}
                                        style={{ width: "100%", padding: "9px 12px 9px 26px", border: `1px solid ${t.bd2}`, borderRadius: 8, fontSize: 13, color: t.tx0, background: t.bg3, outline: "none" }}
                                    />
                                </div>
                            </div>
                        )}

                        <div>
                            <label style={{ fontWeight: 600, fontSize: 12, display: "block", marginBottom: 7, color: t.tx1, fontFamily: t.ffH }}>Solicitudes Especiales</label>
                            <textarea placeholder="Notas adicionales..." value={notasEspeciales} onChange={(e) => setNotasEspeciales(e.target.value)} rows={2}
                                style={{ width: "100%", padding: "9px 12px", border: `1px solid ${t.bd2}`, borderRadius: 8, fontSize: 13, resize: "none", color: t.tx0, background: t.bg3, outline: "none" }}
                            />
                        </div>
                    </>
                )}

                {/* CANTIDAD + CARRITO */}
                <div style={{ display: "flex", gap: 10, marginTop: 10 }}>
                    <div style={{ display: "flex", alignItems: "center", border: `1px solid ${t.bd2}`, borderRadius: 8, background: t.bg3 }}>
                        <button onClick={() => setCantidad(c => Math.max(1, c - 1))} style={{ width: 38, height: 44, background: "none", border: "none", color: t.tx2, cursor: "pointer" }}>&minus;</button>
                        <span style={{ width: 30, textAlign: "center", fontSize: 14, fontWeight: 600, color: t.tx0 }}>{cantidad}</span>
                        <button onClick={() => setCantidad(c => c + 1)} style={{ width: 38, height: 44, background: "none", border: "none", color: t.tx2, cursor: "pointer" }}>+</button>
                    </div>
                    <button
                        onClick={handleAgregarCarrito}
                        onMouseEnter={() => setBtnHover(true)}
                        onMouseLeave={() => setBtnHover(false)}
                        style={{
                            flex: 1, padding: "13px", background: btnHover ? t.sv1 : t.white, color: t.bg0, border: "none", borderRadius: 10,
                            fontSize: 14, fontWeight: 600, cursor: "pointer", transition: "all 0.2s", boxShadow: btnHover ? "0 4px 15px rgba(0,0,0,0.4)" : "none"
                        }}
                    >
                        Agregar al carrito
                    </button>
                </div>
            </div>
        </div>
    );
}

/* ── Toast notification ── */
function mostrarToast(texto) {
    const el = document.createElement("div");
    el.textContent = texto;
    Object.assign(el.style, {
        position: "fixed", bottom: "28px", right: "28px", background: "#fff", color: "#080808",
        padding: "13px 20px", borderRadius: "10px", fontFamily: "Inter, sans-serif", fontSize: "13px",
        fontWeight: "600", boxShadow: "0 8px 32px rgba(0,0,0,0.45)", zIndex: "9999", opacity: "0",
        transition: "opacity 0.2s ease, transform 0.2s ease", transform: "translateY(8px)",
    });
    document.body.appendChild(el);
    requestAnimationFrame(() => { el.style.opacity = "1"; el.style.transform = "translateY(0)"; });
    setTimeout(() => {
        el.style.opacity = "0"; el.style.transform = "translateY(8px)";
        setTimeout(() => el.remove(), 220);
    }, 2600);
}

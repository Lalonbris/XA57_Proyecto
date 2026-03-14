import { useState, useRef } from "react";

const colores = [
    { nombre: "Rojito",       hex: "#E8322A" },
    { nombre: "ETN Gold",     hex: "#b8860b" },
    { nombre: "Primera Plus", hex: "#F0C200" },
    { nombre: "Futura",       hex: "#a855f7" },
];

/* ── Design tokens (mirrors site.css) ── */
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

export default function Configurador({ productoId, productoImagen }) {
    const [colorHex,        setColorHex]        = useState("#F0C200");
    const [colorNombre,     setColorNombre]      = useState("Primera Plus");
    const [numeroSerie,     setNumeroSerie]      = useState("");
    const [notasEspeciales, setNotasEspeciales]  = useState("");
    const [habilitado,      setHabilitado]       = useState(true);
    const [cantidad,        setCantidad]         = useState(1);
    const [textPos,         setTextPos]          = useState({ x: 50, y: 75 });
    const [dragging,        setDragging]         = useState(false);
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

    const handleAgregarCarrito = async () => {
        const datos = { productoId, color: colorNombre, colorHex, numeroSerie, notasEspeciales, cantidad };
        const response = await fetch("/Carrito/Agregar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(datos),
        });
        if (response.ok) mostrarToast("Producto agregado al carrito.");
    };

    /* ── Drag handlers ── */
    const onMouseDown = (e) => {
        e.preventDefault();
        setDragging(true);
        const rect = previewRef.current.getBoundingClientRect();
        setDragOffset({
            x: e.clientX - rect.left - (textPos.x / 100) * rect.width,
            y: e.clientY - rect.top  - (textPos.y / 100) * rect.height,
        });
    };

    const onMouseMove = (e) => {
        if (!dragging) return;
        const rect = previewRef.current.getBoundingClientRect();
        setTextPos({
            x: Math.min(Math.max(((e.clientX - rect.left - dragOffset.x) / rect.width)  * 100, 0), 95),
            y: Math.min(Math.max(((e.clientY - rect.top  - dragOffset.y) / rect.height) * 100, 0), 95),
        });
    };

    const onTouchStart = (e) => {
        const touch = e.touches[0];
        setDragging(true);
        const rect = previewRef.current.getBoundingClientRect();
        setDragOffset({
            x: touch.clientX - rect.left - (textPos.x / 100) * rect.width,
            y: touch.clientY - rect.top  - (textPos.y / 100) * rect.height,
        });
    };

    const onTouchMove = (e) => {
        if (!dragging) return;
        const touch = e.touches[0];
        const rect = previewRef.current.getBoundingClientRect();
        setTextPos({
            x: Math.min(Math.max(((touch.clientX - rect.left - dragOffset.x) / rect.width)  * 100, 0), 95),
            y: Math.min(Math.max(((touch.clientY - rect.top  - dragOffset.y) / rect.height) * 100, 0), 95),
        });
    };

    return (
        <div style={{ fontFamily: t.ffB, color: t.tx0, width: "100%" }}>

            {/* ── PREVIEW ── */}
            <div
                ref={previewRef}
                onMouseMove={onMouseMove}
                onMouseUp={() => setDragging(false)}
                onMouseLeave={() => setDragging(false)}
                onTouchMove={onTouchMove}
                onTouchEnd={() => setDragging(false)}
                style={{
                    marginBottom: 6,
                    position: "relative",
                    borderRadius: 16,
                    overflow: "hidden",
                    background: "#f4f4f4",
                    userSelect: "none",
                    border: `1px solid ${t.bd2}`,
                }}
            >
                <img
                    src={productoImagen}
                    alt="Producto"
                    style={{ width: "100%", display: "block", objectFit: "contain" }}
                />

                {/* Color overlay */}
                <div style={{
                    position: "absolute", inset: 0,
                    backgroundColor: colorHex,
                    mixBlendMode: "hue",
                    opacity: 0.85,
                    transition: "background-color 0.3s",
                    pointerEvents: "none",
                }} />

                {/* Draggable serial number */}
                {numeroSerie && (
                    <div
                        onMouseDown={onMouseDown}
                        onTouchStart={onTouchStart}
                        style={{
                            position: "absolute",
                            left: `${textPos.x}%`,
                            top:  `${textPos.y}%`,
                            transform: "translate(-50%, -50%)",
                            cursor: dragging ? "grabbing" : "grab",
                            userSelect: "none",
                            padding: "4px 10px",
                            background: "rgba(0,0,0,0.65)",
                            color: "#fff",
                            fontWeight: 700,
                            fontFamily: t.ffH,
                            fontSize: 17,
                            borderRadius: 5,
                            letterSpacing: 2,
                            boxShadow: "0 2px 10px rgba(0,0,0,0.5)",
                            border: "1px dashed rgba(255,255,255,0.35)",
                            whiteSpace: "nowrap",
                        }}
                    >
                        {numeroSerie}
                    </div>
                )}
            </div>

            {numeroSerie && (
                <p style={{ textAlign: "center", fontSize: 11, color: t.tx3, marginBottom: 12, fontFamily: t.ffB }}>
                    Arrastra el numero a donde quieras
                </p>
            )}

            <p style={{ textAlign: "center", fontSize: 12, color: t.tx2, marginBottom: 20, fontFamily: t.ffB }}>
                {colorNombre}
            </p>

            {/* ── TOGGLE ── */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
                <div style={{ display: "flex", flexDirection: "column", gap: 2 }}>
                    <span style={{ fontWeight: 600, fontSize: 14, fontFamily: t.ffH, color: t.tx0 }}>
                        Personalize Your Item
                    </span>
                    <span style={{ fontSize: 10, fontWeight: 600, letterSpacing: 2, textTransform: "uppercase", color: t.tx3, fontFamily: t.ffB }}>
                        Studio
                    </span>
                </div>
                <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                    <div
                        onClick={() => setHabilitado(!habilitado)}
                        style={{
                            width: 42, height: 22, borderRadius: 11, cursor: "pointer",
                            background: habilitado ? t.sv1 : t.bg5,
                            position: "relative",
                            transition: "background 0.2s",
                            border: `1px solid ${habilitado ? t.sv1 : t.bd2}`,
                        }}
                    >
                        <div style={{
                            width: 16, height: 16, borderRadius: "50%",
                            background: habilitado ? t.bg0 : t.tx3,
                            position: "absolute", top: 2,
                            left: habilitado ? 22 : 2,
                            transition: "left 0.2s",
                            boxShadow: "0 1px 3px rgba(0,0,0,0.4)",
                        }} />
                    </div>
                    <span style={{ fontSize: 12, color: t.tx2, fontFamily: t.ffB }}>
                        {habilitado ? "Activo" : "Desactivado"}
                    </span>
                </div>
            </div>

            {habilitado && (
                <>
                    {/* Info notice */}
                    <div style={{
                        background: t.bg3,
                        border: `1px solid ${t.bd2}`,
                        borderRadius: 8,
                        padding: "10px 14px",
                        marginBottom: 22,
                        fontSize: 12,
                        color: t.tx2,
                        fontFamily: t.ffB,
                        lineHeight: 1.6,
                    }}>
                        El tamano y modelo del peluche son fijos. La personalizacion aplica al esquema de color, numeracion y notas especiales.
                    </div>

                    {/* Color swatches */}
                    <div style={{ marginBottom: 22 }}>
                        <p style={{
                            fontWeight: 600, fontSize: 12, marginBottom: 10,
                            color: t.tx1, fontFamily: t.ffH, letterSpacing: 0.5,
                        }}>
                            Esquema de Color
                        </p>
                        <div style={{ display: "flex", gap: 10, flexWrap: "wrap" }}>
                            {colores.map((color) => (
                                <div
                                    key={color.nombre}
                                    onClick={() => seleccionarColor(color)}
                                    style={{
                                        cursor: "pointer",
                                        textAlign: "center",
                                        border: colorNombre === color.nombre
                                            ? `2px solid ${t.sv1}`
                                            : "2px solid transparent",
                                        borderRadius: 10,
                                        padding: 3,
                                        transition: "border-color 0.15s",
                                    }}
                                >
                                    <div style={{
                                        width: 72, height: 36, borderRadius: 7,
                                        background: color.hex,
                                        boxShadow: colorNombre === color.nombre
                                            ? `0 0 0 1px ${t.bd3}, 0 4px 12px rgba(0,0,0,0.5)`
                                            : "none",
                                    }} />
                                    <span style={{
                                        fontSize: 10, color: t.tx2, marginTop: 4,
                                        display: "block", fontFamily: t.ffB,
                                    }}>
                                        {color.nombre}
                                    </span>
                                </div>
                            ))}

                            {/* Custom color picker */}
                            <div style={{ textAlign: "center" }}>
                                <div style={{ position: "relative", width: 72, height: 36 }}>
                                    <div style={{
                                        width: 72, height: 36, borderRadius: 7,
                                        background: "linear-gradient(135deg, #ff0000, #ff7700, #ffff00, #00ff00, #0000ff, #8b00ff)",
                                    }} />
                                    <input
                                        type="color"
                                        value={colorHex}
                                        onChange={(e) => handleColorLibre(e.target.value)}
                                        style={{
                                            position: "absolute", inset: 0,
                                            width: "100%", height: "100%",
                                            opacity: 0, cursor: "pointer",
                                        }}
                                    />
                                </div>
                                <span style={{
                                    fontSize: 10, color: t.tx2, marginTop: 4,
                                    display: "block", fontFamily: t.ffB,
                                }}>
                                    Custom
                                </span>
                            </div>
                        </div>
                    </div>

                    {/* Serial number */}
                    <div style={{ marginBottom: 16 }}>
                        <label style={{
                            fontWeight: 600, fontSize: 12, display: "block",
                            marginBottom: 7, color: t.tx1, fontFamily: t.ffH,
                            letterSpacing: 0.5,
                        }}>
                            Numero de Unidad
                            <span style={{ fontWeight: 400, color: t.tx3, fontSize: 11, marginLeft: 8 }}>
                                (ej. numero lateral del autobus)
                            </span>
                        </label>
                        <div style={{ position: "relative" }}>
                            <span style={{
                                position: "absolute", left: 12,
                                top: "50%", transform: "translateY(-50%)",
                                color: t.tx3, fontSize: 13, fontFamily: t.ffB,
                            }}>
                                #
                            </span>
                            <input
                                type="text"
                                placeholder="ej. 9582"
                                value={numeroSerie}
                                onChange={(e) => setNumeroSerie(e.target.value)}
                                style={{
                                    width: "100%",
                                    padding: "9px 12px 9px 26px",
                                    border: `1px solid ${t.bd2}`,
                                    borderRadius: 8,
                                    fontSize: 13,
                                    boxSizing: "border-box",
                                    outline: "none",
                                    color: t.tx0,
                                    background: t.bg3,
                                    fontFamily: t.ffB,
                                    transition: "border-color 0.15s",
                                }}
                                onFocus={e => e.target.style.borderColor = t.bd3}
                                onBlur={e  => e.target.style.borderColor = t.bd2}
                            />
                        </div>
                    </div>

                    {/* Special notes */}
                    <div style={{ marginBottom: 26 }}>
                        <label style={{
                            fontWeight: 600, fontSize: 12, display: "block",
                            marginBottom: 7, color: t.tx1, fontFamily: t.ffH,
                            letterSpacing: 0.5,
                        }}>
                            Solicitudes Especiales
                        </label>
                        <textarea
                            placeholder="Notas para el equipo de personalizacion..."
                            value={notasEspeciales}
                            onChange={(e) => setNotasEspeciales(e.target.value)}
                            rows={3}
                            style={{
                                width: "100%",
                                padding: "9px 12px",
                                border: `1px solid ${t.bd2}`,
                                borderRadius: 8,
                                fontSize: 13,
                                resize: "vertical",
                                boxSizing: "border-box",
                                outline: "none",
                                color: t.tx0,
                                background: t.bg3,
                                fontFamily: t.ffB,
                                lineHeight: 1.55,
                                transition: "border-color 0.15s",
                            }}
                            onFocus={e => e.target.style.borderColor = t.bd3}
                            onBlur={e  => e.target.style.borderColor = t.bd2}
                        />
                    </div>
                </>
            )}

            {/* ── QUANTITY + ADD TO CART ── */}
            <div style={{ display: "flex", gap: 10, alignItems: "center" }}>

                {/* Quantity selector */}
                <div style={{
                    display: "flex",
                    alignItems: "center",
                    border: `1px solid ${t.bd2}`,
                    borderRadius: 8,
                    overflow: "hidden",
                    background: t.bg3,
                    flexShrink: 0,
                }}>
                    <button
                        onClick={() => setCantidad(c => Math.max(1, c - 1))}
                        style={{
                            width: 38, height: 44,
                            background: "none",
                            border: "none",
                            fontSize: 18,
                            cursor: "pointer",
                            color: t.tx2,
                            transition: "color 0.15s",
                        }}
                        onMouseOver={e => e.currentTarget.style.color = t.tx0}
                        onMouseOut={e  => e.currentTarget.style.color = t.tx2}
                    >
                        &minus;
                    </button>
                    <span style={{
                        width: 36, textAlign: "center",
                        fontSize: 14, fontWeight: 600,
                        color: t.tx0, fontFamily: t.ffH,
                    }}>
                        {cantidad}
                    </span>
                    <button
                        onClick={() => setCantidad(c => c + 1)}
                        style={{
                            width: 38, height: 44,
                            background: "none",
                            border: "none",
                            fontSize: 18,
                            cursor: "pointer",
                            color: t.tx2,
                            transition: "color 0.15s",
                        }}
                        onMouseOver={e => e.currentTarget.style.color = t.tx0}
                        onMouseOut={e  => e.currentTarget.style.color = t.tx2}
                    >
                        +
                    </button>
                </div>

                {/* Add to cart button */}
                <button
                    onClick={handleAgregarCarrito}
                    onMouseEnter={() => setBtnHover(true)}
                    onMouseLeave={() => setBtnHover(false)}
                    style={{
                        flex: 1,
                        padding: "13px",
                        background: btnHover ? t.sv1 : t.white,
                        color: t.bg0,
                        border: "none",
                        borderRadius: 10,
                        fontSize: 14,
                        fontWeight: 600,
                        fontFamily: t.ffB,
                        cursor: "pointer",
                        letterSpacing: 0.3,
                        transition: "background 0.2s, transform 0.15s",
                        transform: btnHover ? "translateY(-1px)" : "translateY(0)",
                        boxShadow: btnHover
                            ? "0 6px 20px rgba(0,0,0,0.5)"
                            : "0 2px 8px rgba(0,0,0,0.3)",
                    }}
                >
                    Agregar al carrito
                </button>
            </div>

        </div>
    );
}

/* ── Toast notification ── */
function mostrarToast(texto) {
    const el = document.createElement("div");
    el.textContent = texto;
    Object.assign(el.style, {
        position: "fixed", bottom: "28px", right: "28px",
        background: "#fff", color: "#080808",
        padding: "13px 20px",
        borderRadius: "10px",
        fontFamily: "Inter, sans-serif",
        fontSize: "13px",
        fontWeight: "600",
        boxShadow: "0 8px 32px rgba(0,0,0,0.45)",
        zIndex: "9999",
        opacity: "0",
        transition: "opacity 0.2s ease, transform 0.2s ease",
        transform: "translateY(8px)",
    });
    document.body.appendChild(el);
    requestAnimationFrame(() => {
        el.style.opacity = "1";
        el.style.transform = "translateY(0)";
    });
    setTimeout(() => {
        el.style.opacity = "0";
        el.style.transform = "translateY(8px)";
        setTimeout(() => el.remove(), 220);
    }, 2600);
}

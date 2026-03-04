import { useState, useRef } from "react";

const colores = [
    { nombre: "Rojito", hex: "#E8322A" },
    { nombre: "ETN Gold", hex: "#b8860b" },
    { nombre: "Primera Plus", hex: "#F0C200" },
    { nombre: "Futura", hex: "#a855f7" },
];

export default function Configurador({ productoId, productoImagen }) {
    const [colorHex, setColorHex] = useState("#F0C200");
    const [colorNombre, setColorNombre] = useState("Primera Plus");
    const [numeroSerie, setNumeroSerie] = useState("");
    const [notasEspeciales, setNotasEspeciales] = useState("");
    const [habilitado, setHabilitado] = useState(true);
    const [cantidad, setCantidad] = useState(1);
    const [textPos, setTextPos] = useState({ x: 50, y: 75 }); // posición en % 
    const [dragging, setDragging] = useState(false);
    const [dragOffset, setDragOffset] = useState({ x: 0, y: 0 });
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
        if (response.ok) alert("¡Agregado al carrito!");
    };
    const onMouseDown = (e) => {
        e.preventDefault();
        setDragging(true);
        const rect = previewRef.current.getBoundingClientRect();
        const textX = (textPos.x / 100) * rect.width;
        const textY = (textPos.y / 100) * rect.height;
        setDragOffset({
            x: e.clientX - rect.left - textX,
            y: e.clientY - rect.top - textY,
        });
    };

    const onMouseMove = (e) => {
        if (!dragging) return;
        const rect = previewRef.current.getBoundingClientRect();
        const x = ((e.clientX - rect.left - dragOffset.x) / rect.width) * 100;
        const y = ((e.clientY - rect.top - dragOffset.y) / rect.height) * 100;
        setTextPos({
            x: Math.min(Math.max(x, 0), 95),
            y: Math.min(Math.max(y, 0), 95),
        });
    };

    const onMouseUp = () => setDragging(false);

    // Soporte táctil (celular)
    const onTouchStart = (e) => {
        const touch = e.touches[0];
        setDragging(true);
        const rect = previewRef.current.getBoundingClientRect();
        const textX = (textPos.x / 100) * rect.width;
        const textY = (textPos.y / 100) * rect.height;
        setDragOffset({
            x: touch.clientX - rect.left - textX,
            y: touch.clientY - rect.top - textY,
        });
    };

    const onTouchMove = (e) => {
        if (!dragging) return;
        const touch = e.touches[0];
        const rect = previewRef.current.getBoundingClientRect();
        const x = ((touch.clientX - rect.left - dragOffset.x) / rect.width) * 100;
        const y = ((touch.clientY - rect.top - dragOffset.y) / rect.height) * 100;
        setTextPos({
            x: Math.min(Math.max(x, 0), 95),
            y: Math.min(Math.max(y, 0), 95),
        });
    };

    return (
        <div style={{ fontFamily: "'Segoe UI', sans-serif" }}>

            {/* ── PREVIEW DE IMAGEN ── */}
            <div
                ref={previewRef}
                onMouseMove={onMouseMove}
                onMouseUp={onMouseUp}
                onMouseLeave={onMouseUp}
                onTouchMove={onTouchMove}
                onTouchEnd={onMouseUp}
                style={{ marginBottom: 8, position: "relative", borderRadius: 16, overflow: "hidden", background: "#fff", userSelect: "none" }}
            >
                <img
                    src={productoImagen}
                    alt="Bus Plushie"
                    style={{ width: "100%", display: "block", objectFit: "contain" }}
                />

                {/* Overlay de color */}
                <div style={{
                    position: "absolute", top: 0, left: 0,
                    width: "100%", height: "100%",
                    backgroundColor: colorHex,
                    mixBlendMode: "hue",
                    opacity: 0.85,
                    transition: "background-color 0.3s",
                    pointerEvents: "none",
                }} />

                {/* Texto arrastrable — solo se muestra si hay número */}
                {numeroSerie && (
                    <div
                        onMouseDown={onMouseDown}
                        onTouchStart={onTouchStart}
                        style={{
                            position: "absolute",
                            left: `${textPos.x}%`,
                            top: `${textPos.y}%`,
                            transform: "translate(-50%, -50%)",
                            cursor: dragging ? "grabbing" : "grab",
                            userSelect: "none",
                            padding: "4px 10px",
                            background: "rgba(0,0,0,0.55)",
                            color: "#fff",
                            fontWeight: 700,
                            fontSize: 18,
                            borderRadius: 6,
                            letterSpacing: 2,
                            boxShadow: "0 2px 8px rgba(0,0,0,0.4)",
                            border: "1.5px dashed rgba(255,255,255,0.5)",
                            whiteSpace: "nowrap",
                        }}
                    >
                        {numeroSerie}
                    </div>
                )}
            </div>

            {numeroSerie && (
                <p style={{ textAlign: "center", fontSize: 11, color: "#9ca3af", marginBottom: 16 }}>
                    Arrastra el número a donde quieras
                </p>
            )}
            <p style={{ textAlign: "center", fontSize: 13, color: "#9ca3af", marginBottom: 20 }}>
                {colorNombre}
            </p>
            {/* Toggle */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
                <span style={{ fontWeight: 700, fontSize: 15 }}>
                    Personalize Your Item{" "}
                    <span style={{ background: "#dbeafe", color: "#2563eb", borderRadius: 4, padding: "2px 8px", fontSize: 11, fontWeight: 600 }}>NEW</span>
                </span>
                <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                    <div onClick={() => setHabilitado(!habilitado)} style={{
                        width: 44, height: 24, borderRadius: 12, cursor: "pointer",
                        background: habilitado ? "#f59e0b" : "#d1d5db",
                        position: "relative", transition: "background 0.2s"
                    }}>
                        <div style={{
                            width: 18, height: 18, borderRadius: "50%", background: "#fff",
                            position: "absolute", top: 3,
                            left: habilitado ? 23 : 3, transition: "left 0.2s",
                            boxShadow: "0 1px 3px rgba(0,0,0,0.2)"
                        }} />
                    </div>
                    <span style={{ fontSize: 13, color: "#374151" }}>{habilitado ? "Enable" : "Disabled"}</span>
                </div>
            </div>

            {habilitado && (
                <>
                    {/* Nota */}
                    <div style={{
                        background: "#eff6ff", border: "1px solid #bfdbfe", borderRadius: 8,
                        padding: "10px 14px", marginBottom: 20, fontSize: 13, color: "#1e40af",
                        display: "flex", gap: 8, alignItems: "flex-start"
                    }}>
                        <span>ℹ️</span>
                        <span>Please note: The plushie size and bus model are fixed. Customization applies only to the scheme, numbering, and optional notes.</span>
                    </div>

                    {/* Colores */}
                    <div style={{ marginBottom: 20 }}>
                        <p style={{ fontWeight: 600, fontSize: 14, marginBottom: 10, color: "#111" }}>Chromatic/Paint Scheme</p>
                        <div style={{ display: "flex", gap: 10, flexWrap: "wrap" }}>
                            {colores.map((color) => (
                                <div key={color.nombre} onClick={() => seleccionarColor(color)} style={{
                                    cursor: "pointer", textAlign: "center",
                                    border: colorNombre === color.nombre ? "2px solid #111" : "2px solid transparent",
                                    borderRadius: 10, padding: 3,
                                }}>
                                    <div style={{
                                        width: 80, height: 40, borderRadius: 8, background: color.hex,
                                        boxShadow: colorNombre === color.nombre ? "0 2px 8px rgba(0,0,0,0.25)" : "none"
                                    }} />
                                    <span style={{ fontSize: 11, color: "#374151", marginTop: 4, display: "block" }}>{color.nombre}</span>
                                </div>
                            ))}

                            {/* Color libre */}
                            <div style={{ textAlign: "center" }}>
                                <div style={{ position: "relative", width: 80, height: 40 }}>
                                    <div style={{
                                        width: 80, height: 40, borderRadius: 8,
                                        background: "linear-gradient(135deg, #ff0000, #ff7700, #ffff00, #00ff00, #0000ff, #8b00ff)",
                                    }} />
                                    <input type="color" value={colorHex}
                                        onChange={(e) => handleColorLibre(e.target.value)}
                                        style={{ position: "absolute", top: 0, left: 0, width: "100%", height: "100%", opacity: 0, cursor: "pointer" }}
                                    />
                                </div>
                                <span style={{ fontSize: 11, color: "#374151", marginTop: 4, display: "block" }}>Custom</span>
                            </div>
                        </div>
                    </div>

                    {/* Número de serie */}
                    <div style={{ marginBottom: 16 }}>
                        <label style={{ fontWeight: 600, fontSize: 14, display: "block", marginBottom: 6, color: "#111" }}>
                            Unit Serial Number <span style={{ fontWeight: 400, color: "#9ca3af", fontSize: 12 }}>(e.g. Side of bus number)</span>
                        </label>
                        <div style={{ position: "relative" }}>
                            <span style={{ position: "absolute", left: 12, top: "50%", transform: "translateY(-50%)", color: "#9ca3af", fontSize: 14 }}>#</span>
                            <input type="text" placeholder="e.g. 9582" value={numeroSerie}
                                onChange={(e) => setNumeroSerie(e.target.value)}
                                style={{ width: "100%", padding: "10px 12px 10px 28px", border: "1px solid #e5e7eb", borderRadius: 8, fontSize: 14, boxSizing: "border-box", outline: "none", color: "#111" }}
                            />
                        </div>
                    </div>

                    {/* Notas */}
                    <div style={{ marginBottom: 24 }}>
                        <label style={{ fontWeight: 600, fontSize: 14, display: "block", marginBottom: 6, color: "#111" }}>Special Requests</label>
                        <textarea placeholder="Any specific notes for the customization team..."
                            value={notasEspeciales} onChange={(e) => setNotasEspeciales(e.target.value)}
                            rows={3} style={{ width: "100%", padding: "10px 12px", border: "1px solid #e5e7eb", borderRadius: 8, fontSize: 14, resize: "vertical", boxSizing: "border-box", outline: "none", color: "#111", fontFamily: "inherit" }}
                        />
                    </div>
                </>
            )}

            {/* Cantidad + Carrito */}
            <div style={{ display: "flex", gap: 12, alignItems: "center" }}>
                <div style={{ display: "flex", alignItems: "center", border: "1px solid #e5e7eb", borderRadius: 8, overflow: "hidden" }}>
                    <button onClick={() => setCantidad(c => Math.max(1, c - 1))}
                        style={{ width: 40, height: 48, background: "#fff", border: "none", fontSize: 18, cursor: "pointer", color: "#374151" }}>−</button>
                    <span style={{ width: 40, textAlign: "center", fontSize: 15, fontWeight: 600 }}>{cantidad}</span>
                    <button onClick={() => setCantidad(c => c + 1)}
                        style={{ width: 40, height: 48, background: "#fff", border: "none", fontSize: 18, cursor: "pointer", color: "#374151" }}>+</button>
                </div>
                <button onClick={handleAgregarCarrito} style={{
                    flex: 1, padding: "14px", background: "#f59e0b", color: "#fff",
                    border: "none", borderRadius: 10, fontSize: 15, fontWeight: 700, cursor: "pointer",
                    boxShadow: "0 2px 8px rgba(245,158,11,0.4)"
                }}
                    onMouseOver={e => e.target.style.background = "#d97706"}
                    onMouseOut={e => e.target.style.background = "#f59e0b"}
                >🛒 Add to Cart</button>
            </div>

        </div>
    );
}
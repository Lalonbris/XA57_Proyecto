import React, { useState } from 'react';
import { validarConfiguracion } from '../../utils/validaciones';

const AgregarCarritoBtn = ({ configuracion, tipoProducto, onValidationFailed, t }) => {
    const [isLoading, setIsLoading] = useState(false);

    const handleClick = async () => {
        const errores = validarConfiguracion(configuracion, tipoProducto);
        onValidationFailed(errores); 
        
        if (Object.keys(errores).length > 0) {
            mostrarToast("Por favor, corrige los errores antes de continuar.", true, t);
            return;
        }

        setIsLoading(true);
        try {
            const response = await fetch('/Carrito/Agregar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(configuracion),
            });

            if (response.ok) {
                mostrarToast('¡Producto agregado al carrito!', false, t);
                if (window.actualizarIconoCarrito) window.actualizarIconoCarrito();
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Error al agregar el producto.');
            }
        } catch (error) {
            mostrarToast(error.message, true, t);
        } finally {
            setIsLoading(false);
        }
    };

    const btnStyle = {
        flex: 1, padding: "13px", background: isLoading ? t.bg4 : t.white, 
        color: t.bg0, border: "none", borderRadius: 10,
        fontSize: 14, fontWeight: 600, cursor: isLoading ? "not-allowed" : "pointer", 
        transition: "all 0.2s"
    };

    return (
        <button onClick={handleClick} disabled={isLoading} style={btnStyle}>
            {isLoading ? 'Agregando...' : 'Agregar al carrito'}
        </button>
    );
};

function mostrarToast(texto, esError = false, t) {
    const el = document.createElement("div");
    el.textContent = texto;
    Object.assign(el.style, {
        position: "fixed", bottom: "28px", right: "28px", 
        background: esError ? "#ef4444" : t.white, 
        color: esError ? t.white : t.bg0,
        padding: "13px 20px", borderRadius: "10px", fontFamily: t.ffB, fontSize: "13px",
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

export default AgregarCarritoBtn;

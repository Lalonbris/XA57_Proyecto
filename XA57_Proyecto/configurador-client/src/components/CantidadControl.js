import { useState } from "react";

/**
 * Componente de control de cantidad
 * Cumple con EPIC-07-08
 * @param {Object} props - Props del componente
 * @param {number} props.cantidad - Cantidad actual
 * @param {Function} props.onChange - Función callback cuando cambia la cantidad
 */
export const CantidadControl = ({ cantidad, onChange }) => {
  const handleDecrement = () => {
    // Valor mínimo: 1 (no permite reducir por debajo) - EPIC-07-08
    if (cantidad > 1) {
      onChange(cantidad - 1);
    }
  };

  const handleIncrement = () => {
    onChange(cantidad + 1);
  };

  const handleInputChange = (e) => {
    const valor = parseInt(e.target.value) || 1;
    onChange(Math.max(1, valor)); // Asegurar mínimo de 1
  };

  return (
    <div className="flex items-center space-x-3">
      <button
        onClick={handleDecrement}
        disabled={cantidad <= 1}
        className={`w-10 h-10 border border-gray-300 rounded-md 
                  ${cantidad <= 1 ? "opacity-50 cursor-not-allowed" : ""}
                  hover:${cantidad <= 1 ? "" : "border-gray-400"}`}
      >
        −
      </button>
      
      <input
        type="number"
        value={cantidad}
        min="1"
        onChange={handleInputChange}
        className="w-16 text-center border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
      />
      
      <button
        onClick={handleIncrement}
        className="w-10 h-10 border border-gray-300 rounded-md hover:border-gray-400"
      >
        +
      </button>
    </div>
  );
};
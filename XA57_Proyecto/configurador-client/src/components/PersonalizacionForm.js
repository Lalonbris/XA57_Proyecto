import { useState } from "react";
import { validarConfiguracion } from "../utils/validaciones";

/**
 * Componente de formulario de personalización opcional
 * Cumple con EPIC-07-06 y especifica validaciones U-02, RN-03, RN-04
 * @param {Object} props - Props del componente
 * @param {Object} props.tipoProducto - Información del tipo de producto con flags
 * @param {Object} props.valores - Valores actuales de personalización
 * @param {Function} props.onChange - Función callback cuando cambian los valores
 */
export const PersonalizacionForm = ({ tipoProducto, valores, onChange }) => {
  const [errores, setErrores] = useState({});

  const handleChange = (campo, valor) => {
    // Actualizar el valor del campo
    onChange({
      ...valores,
      [campo]: valor === "" ? null : valor
    });
    
    // Validar en tiempo real si hay cambios
    if (tipoProducto) {
      const nuevosErrores = validarConfiguracion(
        { ...valores, [campo]: valor }, 
        tipoProducto
      );
      setErrores(nuevosErrores);
    }
  };

  // Validar todo el formulario cuando se intente enviar
  const validarFormulario = () => {
    if (!tipoProducto) return true;
    
    const nuevosErrores = validarConfiguracion(valores, tipoProducto);
    setErrores(nuevosErrores);
    return Object.keys(nuevosErrores).length === 0;
  };

  return (
    <div className="space-y-4">
      {/* Nombre del operador (opcional) */}
      {tipoProducto?.permiteNombre && (
        <div>
          <label className="block font-medium text-sm text-gray-700 mb-1">
            Nombre del operador (opcional)
          </label>
          <div className="relative">
            <input
              type="text"
              placeholder="Ej: Juan Pérez"
              maxLength={tipoProducto.maxCaracteres}
              value={valores.nombreOperador || ""}
              onChange={(e) => handleChange("nombreOperador", e.target.value)}
              className={`block w-full px-4 py-2 mt-1 border border-gray-300 rounded-md 
                        ${errores.nombreOperador ? "border-red-500" : ""}
                        ${errores.nombreOperador ? "bg-red-50" : ""}`}
            />
            {errores.nombreOperador && (
              <p className="mt-1 text-sm text-red-600">{errores.nombreOperador}</p>
            )}
            {/* Contador de caracteres */}
            <div className="mt-1 text-xs text-right">
              {valores.nombreOperador?.length || 0}/{tipoProducto.maxCaracteres}
              {valores.nombreOperador?.length && 
                valores.nombreOperador.length > tipoProducto.maxCaracteres && 
                <span className="ml-1 text-red-500 font-medium">¡Límite excedido!</span>}
            </div>
          </div>
        </div>
      )}

      {/* Número económico (opcional) */}
      {tipoProducto?.permiteNumeroEconomico && (
        <div>
          <label className="block font-medium text-sm text-gray-700 mb-1">
            Número económico (opcional)
          </label>
          <div className="relative">
            <input
              type="text"
              placeholder="Ej: 9582"
              maxLength={tipoProducto.maxCaracteres}
              value={valores.numeroEconomico || ""}
              onChange={(e) => handleChange("numeroEconomico", e.target.value)}
              className={`block w-full px-4 py-2 mt-1 border border-gray-300 rounded-md 
                        ${errores.numeroEconomico ? "border-red-500" : ""}
                        ${errores.numeroEconomico ? "bg-red-50" : ""}`}
            />
            {errores.numeroEconomico && (
              <p className="mt-1 text-sm text-red-600">{errores.numeroEconomico}</p>
            )}
            {/* Contador de caracteres */}
            <div className="mt-1 text-xs text-right">
              {valores.numeroEconomico?.length || 0}/{tipoProducto.maxCaracteres}
              {valores.numeroEconomico?.length && 
                valores.numeroEconomico.length > tipoProducto.maxCaracteres && 
                <span className="ml-1 text-red-500 font-medium">¡Límite excedido!</span>}
            </div>
          </div>
        </div>
      )}

      {/* Ruta (opcional) */}
      {tipoProducto?.permiteRuta && (
        <div>
          <label className="block font-medium text-sm text-gray-700 mb-1">
            Ruta (opcional)
          </label>
          <div className="relative">
            <input
              type="text"
              placeholder="Ej: México - Guadalajara"
              maxLength={tipoProducto.maxCaracteres}
              value={valores.ruta || ""}
              onChange={(e) => handleChange("ruta", e.target.value)}
              className={`block w-full px-4 py-2 mt-1 border border-gray-300 rounded-md 
                        ${errores.ruta ? "border-red-500" : ""}
                        ${errores.ruta ? "bg-red-50" : ""}`}
            />
            {errores.ruta && (
              <p className="mt-1 text-sm text-red-600">{errores.ruta}</p>
            )}
            {/* Contador de caracteres */}
            <div className="mt-1 text-xs text-right">
              {valores.ruta?.length || 0}/{tipoProducto.maxCaracteres}
              {valores.ruta?.length && 
                valores.ruta.length > tipoProducto.maxCaracteres && 
                <span className="ml-1 text-red-500 font-medium">¡Límite excedido!</span>}
            </div>
          </div>
        </div>
      )}

      {/* Notas especiales (siempre disponible) */}
      <div>
        <label className="block font-medium text-sm text-gray-700 mb-1">
          Notas especiales (opcional)
        </label>
        <div className="relative">
          <textarea
            placeholder="Ej: Requiere manejo especial..."
            rows={3}
            value={valores.notasEspeciales || ""}
            onChange={(e) => handleChange("notasEspeciales", e.target.value)}
            className={`block w-full px-4 py-2 mt-1 border border-gray-300 rounded-md 
                      ${errores.notasEspeciales ? "border-red-500" : ""}
                      ${errores.notasEspeciales ? "bg-red-50" : ""}`}
          />
          {errores.notasEspeciales && (
            <p className="mt-1 text-sm text-red-600">{errores.notasEspeciales}</p>
          )}
          {/* Contador de caracteres para notas (usando el mismo límite) */}
          <div className="mt-1 text-xs text-right">
            {valores.notasEspeciales?.length || 0}/{tipoProducto?.maxCaracteres || 20}
            {valores.notasEspeciales?.length && 
              valores.notasEspeciales.length > (tipoProducto?.maxCaracteres || 20) && 
              <span className="ml-1 text-red-500 font-medium">¡Límite excedido!</span>}
          </div>
        </div>
      </div>
    </div>
  );
};
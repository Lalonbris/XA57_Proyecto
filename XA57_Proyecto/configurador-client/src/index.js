import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

const contenedor = document.getElementById("configurador-root");
if (contenedor) {
    const data = { ...contenedor.dataset };
    const root = ReactDOM.createRoot(contenedor);
    root.render(<App {...data} />);
}
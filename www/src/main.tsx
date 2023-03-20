import "@ant-design/flowchart/dist/index.css";
import "antd/dist/reset.css";
import "rc-footer/assets/index.css";
import React from "react";
import ReactDOM from "react-dom/client";
// import "virtual:windi-devtools";
// import "virtual:windi.css";
import App from "./App";
// import "./index.css";
import "./i18n";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

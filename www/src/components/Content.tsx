import { Layout } from "antd";
import { CSSProperties } from "react";
import { Outlet } from "react-router-dom";

const { Content: AntContent } = Layout;

const contentStyle: CSSProperties = {
  // textAlign: "center",
  // minHeight: "120",
  // lineHeight: "120px",
  // color: "#fff",
  // backgroundColor: "#108ee9",
};

const Content = () => {
  return (
    <AntContent>
      <div
        style={{
          display: "flex",
          width: "100%",
          height: "100%",
          flexDirection: "column",
          justifyContent: "start",
          // alignItems: "center",
          overflow: "auto",
          padding: "1rem",
        }}
      >
        <Outlet />
      </div>
    </AntContent>
  );
};

export default Content;

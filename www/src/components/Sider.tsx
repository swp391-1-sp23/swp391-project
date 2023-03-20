import { Layout } from "antd";
import { CSSProperties } from "react";

const { Sider: AntSider } = Layout;

const siderStyle: CSSProperties = {
  textAlign: "center",
  lineHeight: "120px",
  color: "#fff",
  backgroundColor: "#3ba0e9",
};

const Sider = () => {
  return (
    <AntSider style={siderStyle} collapsible={true} trigger={null}>
      Sider
    </AntSider>
  );
};

export default Sider;

import { Result } from "antd";
import { Navigate } from "react-router-dom";

const DashboardPage = () => {
  return <Navigate to={Math.random().toString()} />;
};

export default DashboardPage;

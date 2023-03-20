import { Button, Result, Row, Space } from "antd";
import { createBrowserRouter, Navigate, redirect } from "react-router-dom";
import AccountManagementPage from "src/pages/AccountManagementPage";
import BrandManagementPage from "src/pages/BrandManagementPage";
import CartPage from "src/pages/CartPage";
import DashboardPage from "src/pages/DashboardPage";
import ManagementPage from "src/pages/ManagementPage";
import OrderPage from "src/pages/OrderPage";
import ProductManagementPage from "src/pages/ProductManagementPage";
import ProductPage from "src/pages/ProductPage";
import ProfilePage from "src/pages/ProfilePage";
import RootPage from "src/pages/RootPage";
import StorePage from "src/pages/StorePage";
import TestPage from "src/pages/TestPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <RootPage />,
    children: [
      {
        index: true,
        element: <Navigate to={"store"} replace={true} />,
      },
      {
        path: "store",
        element: <StorePage />,
      },
      {
        path: "product/:productId",
        element: <ProductPage />,
      },
      {
        path: "cart",
        element: <CartPage />,
      },
      {
        path: "order",
        element: <OrderPage />,
      },
      {
        path: "management",
        element: <ManagementPage />,
        children: [
          {
            index: true,
            element: <Navigate to={"dashboard"} replace={true} />,
          },
          {
            path: "dashboard",
            element: <DashboardPage />,
          },
          {
            path: "account",
            element: <AccountManagementPage />,
          },
          {
            path: "brand",
            element: <BrandManagementPage />,
          },
          {
            path: "product",
            element: <ProductManagementPage />,
          },
        ],
      },
      {
        path: "profile",
        element: <ProfilePage />,
      },
      {
        path: "test",
        element: <TestPage />,
      },
      {
        path: "*",
        element: (
          <Row align={"middle"} justify={"center"}>
            <Space direction={"vertical"} align={"center"}>
              <Result status={"404"} title={404} subTitle={"Not Found"} />
              <Button
                type={"primary"}
                onClick={() => (window.location.href = "/")}
              >
                Back to home
              </Button>
            </Space>
          </Row>
        ),
      },
    ],
  },
]);

export default router;

import {
  AppstoreOutlined,
  ShoppingCartOutlined,
  TagOutlined,
  UserOutlined,
} from "@ant-design/icons";
import { useQuery } from "@tanstack/react-query";
import {
  Badge,
  Button,
  Col,
  Dropdown,
  Layout,
  Row,
  Space,
  theme,
  Typography,
} from "antd";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useProfileStore from "src/hooks/states/useProfileStore";
import useTokenStore from "src/hooks/states/useTokenStore";
import Login from "./Login";
import Register from "./Register";

const { Header: AntHeader } = Layout;

const Header = () => {
  const { t } = useTranslation();
  const { token, resetToken } = useTokenStore();
  const { profile, resetProfile } = useProfileStore();
  const navigate = useNavigate();
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const logout = () => {
    Promise.all([resetToken(), resetProfile(), navigate("/")]);
  };

  const cartQuery = useQuery({
    enabled: !!token,
    ...queryKeys.cartQueryKeys.get,
    select: ({ data: { data } }) => data,
  });

  return (
    <AntHeader
      style={{
        backgroundColor: colorBgContainer,
        position: "sticky",
        top: 0,
        zIndex: 1,
        // width: "100%",
      }}
    >
      <Row justify={"space-between"}>
        {/* LOGO */}
        <Col>
          <Space onClick={() => navigate("/")}>
            <Typography.Title level={3}>
              {t("HEADER.LOGO.NAME")}
            </Typography.Title>
          </Space>
        </Col>
        {/* AUTH */}
        <Col>
          <Row>
            {token === null && (
              <Col>
                <Space>
                  <Login />
                  <Register />
                </Space>
              </Col>
            )}
            {token !== null && (
              <Col>
                <Space>
                  {profile?.role === "Customer" && (
                    <Badge count={cartQuery.data?.length}>
                      <Button
                        type={"text"}
                        shape={"round"}
                        icon={<ShoppingCartOutlined />}
                        onClick={() => navigate(["cart"].join("/"))}
                      />
                    </Badge>
                  )}
                  <Button
                    type={"text"}
                    shape={"round"}
                    icon={<UserOutlined />}
                    onClick={() => navigate(["profile"].join("/"))}
                  />
                  {profile?.role === "Shop" && (
                    <Dropdown.Button
                      type={"text"}
                      onClick={() => navigate("/management")}
                      menu={{
                        items: [
                          {
                            key: "account",
                            label: t("HEADER.AUTH.MANAGEMENT.ACCOUNT"),
                            icon: <UserOutlined />,
                            onClick: () =>
                              navigate(["/management", "account"].join("/")),
                          },
                          {
                            key: "brand",
                            label: t("HEADER.AUTH.MANAGEMENT.BRAND"),
                            icon: <TagOutlined />,
                            onClick: () =>
                              navigate(["/management", "brand"].join("/")),
                          },
                          {
                            key: "product",
                            label: t("HEADER.AUTH.MANAGEMENT.PRODUCT"),
                            icon: <AppstoreOutlined />,
                            onClick: () =>
                              navigate(["/management", "product"].join("/")),
                          },
                        ],
                      }}
                    >
                      {t("HEADER.AUTH.MANAGEMENT")}
                    </Dropdown.Button>
                  )}
                  <Button type={"text"} onClick={logout}>
                    {t("HEADER.AUTH.LOGOUT")}
                  </Button>
                </Space>
              </Col>
            )}
          </Row>
        </Col>
        {/* ACTIONS */}
      </Row>
    </AntHeader>
  );
};

export default Header;

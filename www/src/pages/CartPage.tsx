import { DollarOutlined } from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Button,
  Card,
  Col,
  Form,
  InputNumber,
  List,
  Modal,
  Row,
  Select,
  Space,
  Typography,
} from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const CartPage = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();
  const query = useQuery({
    ...queryKeys.cartQueryKeys.get,
    select: ({ data: { data } }) => data,
  });

  const quantityMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["UpdateCartProductQuantityDto"] & {
        cartId: string;
      }
    ) => {
      const putCart = apiRequest
        .path("/api/cart/{cartId}")
        .method("put")
        .create();

      return await putCart(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.cartQueryKeys.get._def,
      });
    },
  });

  const [orderModal, setOrderModal] = useState(false);

  const addressQuery = useQuery({
    ...queryKeys.addressQueryKeys.get,
    select: ({ data: { data } }) => data,
  });

  const [orderForm] = Form.useForm();

  const orderMutation = useMutation({
    mutationFn: async (params: components["schemas"]["AddOrderDto"]) => {
      const postOrder = await apiRequest
        .path("/api/order")
        .method("post")
        .create();

      return await postOrder(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.cartQueryKeys.get._def,
      });
    },
  });

  return (
    <Row justify={"center"}>
      <Col xs={22} sm={20} md={18}>
        <Space direction={"vertical"} style={{ width: "100%" }}>
          <Row justify={"space-between"} align={"middle"}>
            <Typography.Title level={2}>{t("CART.TITLE")}</Typography.Title>
            {/* <Button type={"primary"}>{t("CART.ORDER")}</Button> */}
          </Row>
          <Card>
            <List
              dataSource={query.data ?? []}
              renderItem={(item) => (
                <List.Item
                  key={item.id}
                  actions={[
                    <InputNumber
                      value={item.quantity}
                      onChange={(value) => {
                        quantityMutation.mutate({
                          cartId: item.id!,
                          cartProductId: item.id,
                          quantity: Number(value),
                        });
                      }}
                    />,
                    <Button icon={<DollarOutlined />}>
                      {t("CART.PRICE.ITEM", {
                        price: item.quantity! * item.product?.price!,
                      })}
                    </Button>,
                  ]}
                >
                  <List.Item.Meta
                    title={
                      <Link to={["/product", item.product?.id].join("/")}>
                        {item.product?.productName}
                      </Link>
                    }
                    description={[
                      item.color?.colorName,
                      item.size?.sizeName,
                    ].join(", ")}
                  />
                </List.Item>
              )}
            />
            <Row justify={"end"} gutter={16}>
              <Button type={"primary"} onClick={() => setOrderModal(true)}>
                {t("CART.BUY", {
                  price: query.data?.reduce(
                    (prev, cur) => prev + cur.product?.price! * cur.quantity!,
                    0
                  ),
                })}
              </Button>
            </Row>
          </Card>
          <Modal
            open={orderModal}
            confirmLoading={orderMutation.isLoading}
            onCancel={() => setOrderModal(false)}
            onOk={() => setOrderModal(false)}
            title={t("FORM.ORDER")}
            footer={null}
          >
            <Form
              form={orderForm}
              onFinish={(values: components["schemas"]["AddOrderDto"]) => {
                orderMutation.mutate({
                  ...values,
                  cartIds: query.data?.map((item) => item.id!),
                });
              }}
            >
              <Form.Item name={"shippingAddressId"}>
                <Select>
                  {addressQuery.data?.map((item) => (
                    <Select.Option value={item.id} key={item.id}>
                      {item.addressName}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item>
                <Space>
                  <Button onClick={() => orderForm.resetFields()}>
                    {t("FORM.ORDER.RESET")}
                  </Button>
                  <Button type={"primary"} htmlType={"submit"}>
                    {t("FORM.ORDER.SUBMIT")}
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Modal>
        </Space>
      </Col>
    </Row>
  );
};

export default CartPage;

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Button,
  Card,
  Col,
  InputNumber,
  List,
  Row,
  Space,
  Typography,
} from "antd";
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
  return (
    <Row justify={"center"}>
      <Col xs={22} sm={20} md={18}>
        <Space direction={"vertical"} style={{ width: "100%" }}>
          <Row justify={"space-between"} align={"middle"}>
            <Typography.Title level={2}>{t("CART.TITLE")}</Typography.Title>
            <Button type={"primary"}>{t("CART.ORDER")}</Button>
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
          </Card>
        </Space>
      </Col>
    </Row>
  );
};

export default CartPage;

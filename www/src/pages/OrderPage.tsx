import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Card, Col, List, Row, Segmented, Space, Typography } from "antd";
import { Trans, useTranslation } from "react-i18next";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const OrderPage = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const orderQuery = useQuery({
    ...queryKeys.orderQueryKeys.get,
    select: ({ data: { data } }) => data,
  });

  const orderMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["UpdateOrderStatusDto"] & {
        orderTag: string;
      }
    ) => {
      const postOrder = apiRequest
        .path("/api/order/{orderTag}")
        .method("put")
        .create();

      return await postOrder(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.orderQueryKeys.get._def,
      });
    },
  });
  return (
    <Row justify={"center"}>
      <Col xs={22} sm={20} md={18}>
        <Space direction={"vertical"} style={{ width: "100%" }}>
          <Row justify={"space-between"} align={"middle"}>
            <Typography.Title level={2}>{t("ORDER.TITLE")}</Typography.Title>
          </Row>
          <Card>
            <List
              dataSource={Object.entries(orderQuery.data ?? {})}
              renderItem={(item) => (
                <List.Item key={item[0]} actions={[]}>
                  <Row gutter={[16, 16]} style={{ width: "100%" }}>
                    <Col xs={24} md={6}>
                      <Typography.Text ellipsis={true}>
                        {item[0]}
                      </Typography.Text>
                    </Col>
                    <Col xs={24} md={8}>
                      <Typography.Title level={5} ellipsis={true}>
                        {item[1]
                          ?.map((item) => item.product?.productName)
                          .join(", ")}
                      </Typography.Title>
                    </Col>
                    <Col xs={24} md={4}>
                      <Typography.Text>
                        {t("ORDER.SHIP_TO", {
                          addressName: item[1]?.at(0)?.address?.addressName,
                        })}
                      </Typography.Text>
                    </Col>
                    <Col xs={24} md={6}>
                      <Space
                        direction={"vertical"}
                        align={"end"}
                        style={{ width: "100%" }}
                      >
                        <Segmented
                          options={[
                            {
                              value: "Created",
                              label: t("ORDER.STATUS.CREATED"),
                              disabled: true,
                            },
                            {
                              value: "Paid",
                              label: t("ORDER.STATUS.PAID"),
                              disabled: item[1]?.at(0)?.status !== "Created",
                            },
                            {
                              value: "Shipped",
                              label: t("ORDER.STATUS.SHIPPED"),
                              disabled: item[1]?.at(0)?.status !== "Paid",
                            },
                          ]}
                          value={item[1]?.at(0)?.status}
                          onChange={(value) =>
                            orderMutation.mutate({
                              orderTag: item[0],
                              status:
                                value as components["schemas"]["OrderStatus"],
                              tag: item[0],
                            })
                          }
                        />
                        {new Date(item[1]?.at(0)?.date!).toLocaleDateString()}
                      </Space>
                    </Col>
                  </Row>
                </List.Item>
              )}
            />
          </Card>
        </Space>
      </Col>
    </Row>
  );
};

export default OrderPage;

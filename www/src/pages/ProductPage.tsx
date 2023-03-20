import { ShoppingCartOutlined } from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Button, Card, Col, Image, Row, Space, Tag, Typography } from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useProfileStore from "src/hooks/states/useProfileStore";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const ProductPage = () => {
  const params = useParams();
  const { profile } = useProfileStore();
  const query = useQuery({
    ...queryKeys.productQueryKeys.getById({ productId: params?.productId! }),
    // enabled: profile?.role === "Customer",
    select: ({ data: { data } }) => data,
  });
  const [selectedColor, setSelectedColor] = useState("");
  const [selectedSize, setSelectedSize] = useState("");

  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async (params: components["schemas"]["AddCartProductDto"]) => {
      const postCart = apiRequest.path("/api/cart").method("post").create();

      return await postCart(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.cartQueryKeys.get._def,
      });
    },
  });
  const { t } = useTranslation();
  return (
    <Row justify={"center"}>
      <Col xs={22} sm={20} md={18}>
        <Space direction={"vertical"} style={{ width: "100%" }}>
          <Row justify={"space-between"} align={"middle"}>
            <Typography.Title level={2}>Product</Typography.Title>
          </Row>
          <Card>
            <Row gutter={16}>
              <Col xs={24} md={12}>
                <Image
                  // height={150}
                  alt={query.data?.productName!}
                  src={
                    (query.data?.images?.at(0) as Record<string, string>)?.[
                      "item2"
                    ]
                  }
                />
              </Col>
              <Col xs={24} md={12}>
                <Space direction={"vertical"}>
                  <Typography.Title level={3}>
                    {query.data?.productName}
                  </Typography.Title>
                  <Tag color={"gold"}>{query.data?.brand?.brandName}</Tag>
                  <Tag color={"green"}>
                    {t("PRODUCT.PRICE", { price: query.data?.price })}
                  </Tag>
                  <Typography.Paragraph>
                    {query.data?.description}
                  </Typography.Paragraph>
                  <Space direction={"vertical"}>
                    <Row gutter={16}>
                      <Col>Color</Col>
                      <Col>
                        {query.data?.colors?.map((item) => (
                          <Tag
                            onClick={() => setSelectedColor(item.id!)}
                            color={
                              item.id === selectedColor ? "blue" : "default"
                            }
                          >
                            {item.colorName}
                          </Tag>
                        ))}
                      </Col>
                    </Row>
                    <Row gutter={16}>
                      <Col>Size</Col>
                      <Col>
                        {query.data?.sizes?.map((item) => (
                          <Tag
                            onClick={() => setSelectedSize(item.id!)}
                            color={
                              item.id === selectedSize ? "blue" : "default"
                            }
                          >
                            {item.sizeName}
                          </Tag>
                        ))}
                      </Col>
                    </Row>
                  </Space>
                  <Space>
                    {profile?.role === "Customer" && (
                      <Button
                        type={"primary"}
                        icon={<ShoppingCartOutlined />}
                        onClick={() => {
                          const product = query.data;
                          const inStocks = Object.entries(
                            (product?.inStocks as {
                              [key: string]: Record<string, string>;
                            }) ?? {}
                          );
                          // console.log(inStocks);
                          const stock = inStocks.find(
                            (item) =>
                              item[1]["item1"] === selectedColor &&
                              item[1]["item2"] === selectedSize
                          );

                          // console.log(stock);
                          if (!!stock)
                            mutation.mutate({
                              productInStockId: stock[0],
                              quantity: 1,
                            });
                        }}
                      >
                        Add to cart
                      </Button>
                    )}
                  </Space>
                </Space>
              </Col>
            </Row>
          </Card>
        </Space>
      </Col>
    </Row>
  );
};

export default ProductPage;

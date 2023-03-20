import { InfoCircleOutlined, ShoppingCartOutlined } from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useDebounce } from "ahooks";
import { Col, Image, Input, Row, Space } from "antd";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import ProductCard from "src/components/ProductCard";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useProfileStore from "src/hooks/states/useProfileStore";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const StorePage = () => {
  const [searchKey, setSearchKey] = useState("");
  const debounced = useDebounce(searchKey, {
    wait: 500,
  });
  const queryClient = useQueryClient();
  const query = useQuery({
    ...queryKeys.productQueryKeys.get({}),
    select: ({ data: { data } }) => data,
  });
  const { profile } = useProfileStore();
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

  const navigate = useNavigate();
  return (
    <>
      {/* <Carousel /> */}
      <Row justify={"center"}>
        <Col xs={22} sm={20} md={18}>
          <Space style={{ width: "100%" }} direction={"vertical"}>
            <Input.Search allowClear={true} />
            <Row justify={"space-evenly"} gutter={16} style={{ width: "100%" }}>
              {query.data?.length &&
                query.data?.map((item, idx) => (
                  <ProductCard
                    key={idx}
                    cardProps={{
                      actions: [
                        <InfoCircleOutlined
                          onClick={() =>
                            navigate(["/product", item.id].join("/"))
                          }
                        />,
                        profile?.role === "Customer" && (
                          <ShoppingCartOutlined
                            onClick={() => {
                              const stockId = Object.keys(
                                item?.inStocks ?? {}
                              ).at(0);
                              if (!!stockId)
                                mutation.mutate({
                                  productInStockId: Object.keys(
                                    item?.inStocks ?? {}
                                  ).at(0),
                                  quantity: 1,
                                });
                            }}
                            disabled={mutation.isLoading}
                          />
                        ),
                      ],
                      cover: (
                        <Image
                          height={150}
                          alt={item.productName!}
                          src={
                            (item.images?.at(0) as Record<string, string>)?.[
                              "item2"
                            ]
                          }
                        />
                      ),
                    }}
                    productProps={{
                      productName: item.productName!,
                      price: item.price!,
                      brand: item.brand?.brandName!,
                    }}
                  />
                ))}
            </Row>
          </Space>
        </Col>
      </Row>
    </>
  );
};

export default StorePage;

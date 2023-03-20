import {
  DeleteOutlined,
  EditOutlined,
  MinusOutlined,
  PlusOutlined,
} from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Button,
  Dropdown,
  Form,
  Input,
  InputNumber,
  Modal,
  Popconfirm,
  Select,
  Space,
  Table,
  Tag,
  Tooltip,
  Typography,
} from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const ProductManagementPage = () => {
  const { t } = useTranslation();
  const query = useQuery({
    ...queryKeys.productQueryKeys.get({}),
    select: ({ data: { data } }) => data,
  });

  const queryClient = useQueryClient();

  const [selectedProductId, setSelectedProductId] = useState("");

  const [productModal, setProductModal] = useState(false);
  const [colorModal, setColorModal] = useState(false);
  const [sizeModal, setSizeModal] = useState(false);
  const [quantityModal, setQuantityModal] = useState(false);
  const [imageModal, setImageModal] = useState(false);

  const [productForm] = Form.useForm();
  const [colorForm] = Form.useForm();
  const [sizeForm] = Form.useForm();
  const [quantityForm] = Form.useForm();
  const [imageForm] = Form.useForm();

  const productDeleteMutation = useMutation({
    mutationFn: async (productId: string) => {
      const deleteProduct = apiRequest
        .path("/api/product/{productId}")
        .method("delete")
        .create();

      return await deleteProduct({
        productId,
      });
    },
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      }),
  });

  const productPutMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["UpdateProductDto"] & {
        productId: string;
      }
    ) => {
      const putProduct = apiRequest
        .path("/api/product/{productId}")
        .method("put")
        .create();

      return await putProduct(params);
    },
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      }),
  });

  const colorDeleteMutation = useMutation({
    mutationFn: async (params: { productId: string; colorId: string }) => {
      const deleteColor = apiRequest
        .path("/api/product/{productId}/color/{colorId}")
        .method("delete")
        .create();

      return await deleteColor(params);
    },
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      }),
  });

  const colorPostMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["AddProductColorsDto"] & {
        productId: string;
      }
    ) => {
      const postColor = apiRequest
        .path("/api/product/{productId}/color")
        .method("post")
        .create();

      return await postColor(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      });
      setColorModal(false);
    },
  });

  const sizeDeleteMutation = useMutation({
    mutationFn: async (params: { productId: string; sizeId: string }) => {
      const deleteSize = apiRequest
        .path("/api/product/{productId}/size/{sizeId}")
        .method("delete")
        .create();

      return await deleteSize(params);
    },
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      }),
  });

  const sizePostMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["AddProductSizesDto"] & {
        productId: string;
      }
    ) => {
      const postSize = apiRequest
        .path("/api/product/{productId}/size")
        .method("post")
        .create();

      return await postSize(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      });
      setSizeModal(false);
    },
  });

  const quantityPostMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["AddProductQuantityDto"] & {
        productId: string;
      }
    ) => {
      const postQuantity = apiRequest
        .path("/api/product/{productId}/quantity")
        .method("post")
        .create();

      return await postQuantity(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      });
    },
  });

  const quantityPutMutation = useMutation({
    mutationFn: async (
      params: components["schemas"]["UpdateProductQuantityDto"] & {
        productId: string;
      }
    ) => {
      const putQuantity = apiRequest
        .path("/api/product/{productId}/quantity")
        .method("put")
        .create();

      return await putQuantity(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.productQueryKeys.get._def,
      });
    },
  });

  return (
    <Space direction={"vertical"}>
      <Typography.Title level={2}>{t("MANAGEMENT.PRODUCT")}</Typography.Title>
      <Table<components["schemas"]["ProductDto"]>
        dataSource={
          query.data?.map((item) => ({
            ...item,
            actions: null,
            key: item.id,
          })) ?? []
        }
        columns={[
          {
            dataIndex: "id",
            title: t("MANAGEMENT.PRODUCT.TABLE.ID"),
            ellipsis: {
              showTitle: false,
            },
            render: (value) => (
              <Tooltip placement={"topLeft"} title={value}>
                {value}
              </Tooltip>
            ),
          },
          {
            dataIndex: "productName",
            title: t("MANAGEMENT.PRODUCT.TABLE.PRODUCT_NAME"),
            ellipsis: {
              showTitle: false,
            },
            render: (value) => (
              <Tooltip placement={"topLeft"} title={value}>
                {value}
              </Tooltip>
            ),
          },
          {
            dataIndex: "price",
            title: t("MANAGEMENT.PRODUCT.TABLE.PRICE"),
          },
          {
            dataIndex: "colors",
            title: t("MANAGEMENT.PRODUCT.TABLE.COLORS"),
            render: (_, { colors, id }) => (
              <Space direction={"vertical"}>
                {colors?.map((item, idx) => (
                  <Popconfirm
                    key={idx}
                    title={t(
                      "MANAGEMENT.PRODUCT.TABLE.COLORS.DELETE.POP_CONFIRM",
                      {
                        colorName: item.colorName,
                      }
                    )}
                    onConfirm={() =>
                      colorDeleteMutation.mutate({
                        productId: id!,
                        colorId: item.id!,
                      })
                    }
                  >
                    <Tag>{item.colorName}</Tag>
                  </Popconfirm>
                ))}
                <Tag
                  icon={<PlusOutlined />}
                  onClick={() => {
                    setColorModal(true);
                    setSelectedProductId(id!);
                  }}
                >
                  {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS")}
                </Tag>
              </Space>
            ),
          },
          {
            dataIndex: "sizes",
            title: t("MANAGEMENT.PRODUCT.TABLE.SIZES"),
            render: (_, { sizes, id }) => (
              <Space direction={"vertical"}>
                {sizes?.map((item, idx) => (
                  <Popconfirm
                    key={idx}
                    title={t(
                      "MANAGEMENT.PRODUCT.TABLE.SIZES.DELETE.POP_CONFIRM",
                      {
                        sizeName: item.sizeName,
                      }
                    )}
                    onConfirm={() =>
                      sizeDeleteMutation.mutate({
                        productId: id!,
                        sizeId: item.id!,
                      })
                    }
                  >
                    <Tag key={idx}>{item.sizeName}</Tag>
                  </Popconfirm>
                ))}
                <Tag
                  icon={<PlusOutlined />}
                  onClick={() => {
                    setSizeModal(true);
                    setSelectedProductId(id!);
                  }}
                >
                  {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES")}
                </Tag>
                <Modal
                  title={t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES")}
                  open={sizeModal}
                  onCancel={() => {
                    setSizeModal(false);
                  }}
                  footer={null}
                ></Modal>
              </Space>
            ),
          },
          {
            dataIndex: "actions",
            title: t("MANAGEMENT.PRODUCT.TABLE.ACTIONS"),
            render: (value, record, idx) => (
              <Space>
                <Button icon={<EditOutlined />} />
                <Tooltip
                  title={t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_QUANTITY")}
                >
                  <Button
                    icon={<PlusOutlined />}
                    onClick={() => {
                      setQuantityModal(true);
                      setSelectedProductId(record.id!);
                      quantityForm.setFieldValue("productId", record.id!);
                    }}
                  />
                </Tooltip>
                <Popconfirm
                  title={t(
                    "MANAGEMENT.PRODUCT.TABLE.ACTIONS.DELETE.POP_CONFIRM",
                    {
                      productId: record.id,
                    }
                  )}
                  onConfirm={() => productDeleteMutation.mutate(record.id!)}
                >
                  <Button danger={true} icon={<DeleteOutlined />} />
                </Popconfirm>
              </Space>
            ),
          },
        ]}
      />
      <Modal
        title={t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS")}
        open={colorModal}
        onCancel={() => setColorModal(false)}
        footer={null}
        confirmLoading={colorPostMutation.isLoading}
      >
        <Form
          form={colorForm}
          onFinish={(values: components["schemas"]["AddProductColorsDto"]) => {
            colorPostMutation.mutate({
              productId: selectedProductId,
              ...values,
            });
          }}
          initialValues={{
            colorNames: [""],
          }}
          layout={"vertical"}
          disabled={colorPostMutation.isLoading}
        >
          <Form.List
            name={"colorNames"}
            rules={[
              {
                validator: async (_, names) =>
                  (!names || names.length < 1) &&
                  Promise.reject(
                    new Error(
                      t(
                        "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS.COLOR_NAMES.VALIDATE_MESSAGE"
                      )!
                    )
                  ),
              },
            ]}
          >
            {(fields, { add, remove }, { errors }) => (
              <Space direction={"vertical"}>
                {fields.map((item, idx) => (
                  <Form.Item
                    key={item.key}
                    label={
                      idx === 0
                        ? t(
                            "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS.COLOR_NAMES.ITEM"
                          )
                        : ""
                    }
                  >
                    <Space>
                      <Form.Item
                        {...item}
                        validateTrigger={["onChange", "onBlur"]}
                        rules={[
                          {
                            required: true,
                            whitespace: true,
                            message: t(
                              "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS.COLOR_NAME.VALIDATE_MESSAGE"
                            )!,
                          },
                        ]}
                        noStyle
                      >
                        <Input />
                      </Form.Item>
                      {fields.length > 1 && (
                        <Button
                          danger={true}
                          icon={<MinusOutlined />}
                          onClick={() => remove(item.name)}
                        />
                      )}
                    </Space>
                  </Form.Item>
                ))}
                <Form.Item>
                  <Button
                    type={"dashed"}
                    icon={<PlusOutlined />}
                    onClick={() => add()}
                  />
                </Form.Item>
              </Space>
            )}
          </Form.List>
          <Form.Item>
            <Space>
              <Button onClick={() => colorForm.resetFields()}>
                {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS.RESET")}
              </Button>
              <Button type={"primary"} htmlType={"submit"}>
                {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_COLORS.SUBMIT")}
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
      <Modal
        title={t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES")}
        open={sizeModal}
        onCancel={() => setSizeModal(false)}
        footer={null}
        confirmLoading={sizePostMutation.isLoading}
      >
        <Form
          form={sizeForm}
          onFinish={(values: components["schemas"]["AddProductSizesDto"]) => {
            sizePostMutation.mutate({
              productId: selectedProductId,
              ...values,
            });
          }}
          initialValues={{
            sizeNames: [""],
          }}
          layout={"vertical"}
          disabled={sizePostMutation.isLoading}
        >
          <Form.List
            name={"sizeNames"}
            rules={[
              {
                validator: async (_, names) =>
                  (!names || names.length < 1) &&
                  Promise.reject(
                    new Error(
                      t(
                        "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES.SIZE_NAMES.VALIDATE_MESSAGE"
                      )!
                    )
                  ),
              },
            ]}
          >
            {(fields, { add, remove }, { errors }) => (
              <Space direction={"vertical"}>
                {fields.map((item, idx) => (
                  <Form.Item
                    key={item.key}
                    label={
                      idx === 0
                        ? t(
                            "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES.SIZE_NAMES.ITEM"
                          )
                        : ""
                    }
                  >
                    <Space>
                      <Form.Item
                        {...item}
                        validateTrigger={["onChange", "onBlur"]}
                        rules={[
                          {
                            required: true,
                            whitespace: true,
                            message: t(
                              "MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES.SIZE_NAME.VALIDATE_MESSAGE"
                            )!,
                          },
                        ]}
                        noStyle
                      >
                        <Input />
                      </Form.Item>
                      {fields.length > 1 && (
                        <Button
                          danger={true}
                          icon={<MinusOutlined />}
                          onClick={() => remove(item.name)}
                        />
                      )}
                    </Space>
                  </Form.Item>
                ))}
                <Form.Item>
                  <Button
                    type={"dashed"}
                    icon={<PlusOutlined />}
                    onClick={() => add()}
                  />
                </Form.Item>
              </Space>
            )}
          </Form.List>
          <Form.Item>
            <Space>
              <Button onClick={() => sizeForm.resetFields()}>
                {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES.RESET")}
              </Button>
              <Button type={"primary"} htmlType={"submit"}>
                {t("MANAGEMENT.PRODUCT.TABLE.ACTIONS.ADD_SIZES.SUBMIT")}
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
      <Modal
        title={t("FORM.PRODUCT.QUANTITY")}
        open={quantityModal}
        onCancel={() => setQuantityModal(false)}
        // confirmLoading={}
      >
        <Space direction={"vertical"}>
          <Form
            form={quantityForm}
            onFinish={(
              values: components["schemas"]["AddProductQuantityDto"] & {
                productId: string;
              }
            ) => {
              quantityPostMutation.mutate(values);
            }}
            disabled={quantityPostMutation.isLoading}
          >
            <Form.Item
              name={"productId"}
              hidden={true}
              required={true}
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name={"colorId"}
              required={true}
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <Select
                onChange={(value: string) =>
                  quantityForm.setFieldValue("colorId", value)
                }
              >
                {query.data
                  ?.find((p) => p.id === selectedProductId)
                  ?.colors?.map((item, idx) => (
                    <Select.Option value={item.id} key={idx}>
                      {item.colorName}
                    </Select.Option>
                  ))}
              </Select>
            </Form.Item>
            <Form.Item
              name={"sizeId"}
              required={true}
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <Select
                onChange={(value: string) =>
                  quantityForm.setFieldValue("sizeId", value)
                }
              >
                {query.data
                  ?.find((p) => p.id === selectedProductId)
                  ?.sizes?.map((item, idx) => (
                    <Select.Option value={item.id} key={idx}>
                      {item.sizeName}
                    </Select.Option>
                  ))}
              </Select>
            </Form.Item>
            <Form.Item
              name={"quantity"}
              required={true}
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <InputNumber min={0} />
            </Form.Item>

            <Form.Item>
              <Space>
                <Button onClick={() => quantityForm.resetFields()}>
                  {t("FORM.PRODUCT.QUANTITY.RESET")}
                </Button>
                <Button type={"primary"} htmlType={"submit"}>
                  {t("FORM.PRODUCT.QUANTITY.SUBMIT")}
                </Button>
              </Space>
            </Form.Item>
          </Form>
          {(() => {
            const selectedProduct = query.data?.find(
              (p) => p.id === selectedProductId
            );
            const inStocks = Object.entries(
              (selectedProduct?.inStocks as {
                [key: string]: Record<string, string>;
              }) ?? {}
            );
            return (
              // <List
              //   itemLayout={"vertical"}
              //   dataSource={inStocks}
              //   renderItem={([productInStockId, data]) => (
              //     <List.Item key={productInStockId}>
              //       {data?.["item3"]}
              //     </List.Item>
              //   )}
              // />
              <Table
                columns={[
                  {
                    dataIndex: "stockId",
                    title: t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.ID"),
                    ellipsis: true,
                  },
                  {
                    dataIndex: "colorId",
                    title: t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.COLOR"),
                    ellipsis: true,
                    render: (value) =>
                      selectedProduct?.colors?.find((item) => item.id === value)
                        ?.colorName,
                  },
                  {
                    dataIndex: "sizeId",
                    title: t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.SIZE"),
                    ellipsis: true,
                    render: (value) =>
                      selectedProduct?.sizes?.find((item) => item.id === value)
                        ?.sizeName,
                  },
                  {
                    dataIndex: "quantity",
                    title: t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.QUANTITY"),
                  },
                  {
                    dataIndex: "actions",
                    title: t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.ACTIONS"),
                    render: (value, record) => (
                      <Dropdown.Button
                        icon={<PlusOutlined />}
                        // onClick
                        menu={{
                          items: [
                            {
                              key: "1",
                              label: "1",
                              onClick: () =>
                                quantityPutMutation.mutate({
                                  productId: selectedProductId,
                                  productInStockId: record.stockId,
                                  colorId: record.colorId,
                                  sizeId: record.sizeId,
                                  quantity: 1,
                                }),
                            },
                            {
                              key: "10",
                              label: "10",
                              onClick: () =>
                                quantityPutMutation.mutate({
                                  productId: selectedProductId,
                                  productInStockId: record.stockId,
                                  colorId: record.colorId,
                                  sizeId: record.sizeId,
                                  quantity: 10,
                                }),
                            },
                            {
                              key: "50",
                              label: "50",
                              onClick: () =>
                                quantityPutMutation.mutate({
                                  productId: selectedProductId,
                                  productInStockId: record.stockId,
                                  colorId: record.colorId,
                                  sizeId: record.sizeId,
                                  quantity: 50,
                                }),
                            },
                          ],
                        }}
                      >
                        {t("FORM.PRODUCT.QUANTITY.TABLE.COLUMN.ACTIONS.ADD")}
                      </Dropdown.Button>
                    ),
                  },
                ]}
                dataSource={inStocks.map((item) => ({
                  key: item[0],
                  stockId: item[0],
                  colorId: item[1]["item1"],
                  sizeId: item[1]["item2"],
                  quantity: item[1]["item3"],
                  actions: null,
                }))}
              />
            );
          })()}
        </Space>
      </Modal>
    </Space>
  );
};

export default ProductManagementPage;

import {
    DeleteOutlined,
    FileAddOutlined,
    TagOutlined,
  } from "@ant-design/icons";
  import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
  import {
    Avatar,
    Button,
    Col,
    Form,
    Input,
    InputNumber,
    Modal,
    Popconfirm,
    Row,
    Space,
    Table,
    Tooltip,
    Typography,
  } from "antd";
  import { useState } from "react";
  import { useTranslation } from "react-i18next";
  import queryKeys from "src/hooks/apis/keys/queryKeys";
  import apiRequest from "src/utilities/apiRequest";
  import { components } from "src/utilities/apiSchemas";
  
  const BrandManagementPage = () => {
    const { t } = useTranslation();
    const query = useQuery({
      ...queryKeys.brandQueryKeys.get({}),
      select: ({ data: { data } }) => data,
    });
    const queryClient = useQueryClient();
    const brandDeleteMutation = useMutation({
      mutationFn: async (brandId: string) => {
        const deleteBrand = apiRequest
          .path("/api/brand/{brandId}")
          .method("delete")
          .create();
  
        return await deleteBrand({ brandId });
      },
      onSuccess: () => {
        queryClient.invalidateQueries({
          queryKey: queryKeys.brandQueryKeys.get._def,
        });
        queryClient.invalidateQueries({
          queryKey: queryKeys.productQueryKeys.get._def,
        });
      },
    });
    const [brandModal, setBrandModal] = useState(false);
    const [productModal, setProductModal] = useState(false);
    const [brandForm] = Form.useForm();
    const [productForm] = Form.useForm();
    const brandPostMutation = useMutation({
      mutationFn: async (brandName: string) => {
        const postBrand = apiRequest.path("/api/brand").method("post").create();
  
        return await postBrand({ brandName });
      },
      onSuccess: () => {
        queryClient.invalidateQueries({
          queryKey: queryKeys.brandQueryKeys.get._def,
        });
        setBrandModal(false);
      },
    });
    const productPostMutation = useMutation({
      mutationFn: async (params: {
        brandId: string;
        productName: string;
        price: number;
        description: string;
      }) => {
        const postProduct = apiRequest
          .path("/api/brand/{brandId}/product")
          .method("post")
          .create();
  
        return await postProduct(params);
      },
      onSuccess: () => {
        queryClient.invalidateQueries({
          queryKey: queryKeys.productQueryKeys.get._def,
        });
        setProductModal(false);
      },
    });
    return (
      <Space direction={"vertical"}>
        <Row justify={"space-between"}>
          <Col>
            <Typography.Title level={2}>{t("MANAGEMENT.BRAND")}</Typography.Title>
          </Col>
          <Col>
            <Button
              type={"primary"}
              icon={<TagOutlined />}
              onClick={() => setBrandModal(true)}
            >
              {t("MANAGEMENT.BRAND.ADD")}
            </Button>
          </Col>
        </Row>
        <Table<components["schemas"]["BrandDto"] & { actions: null }>
          dataSource={
            query.data?.map((item) => ({
              ...item,
              actions: null,
              key: item.id!,
            })) ?? []
          }
          columns={[
            {
              dataIndex: "id",
              title: t("MANAGEMENT.BRAND.TABLE.COLUMN.ID"),
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
              dataIndex: ["logo", "item2"],
              title: t("MANAGEMENT.BRAND.TABLE.COLUMN.LOGO"),
              render: (value) => <Avatar src={value} />,
            },
            {
              dataIndex: "brandName",
              title: t("MANAGEMENT.BRAND.TABLE.COLUMN.BRAND_NAME"),
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
              dataIndex: "actions",
              title: t("MANAGEMENT.BRAND.TABLE.COLUMN.ACTIONS"),
              render: (value, record, idx) => (
                <Space>
                  <Tooltip
                    title={t("MANAGEMENT.BRAND.TABLE.COLUMN.ACTIONS.ADD_PRODUCT")}
                  >
                    <Button
                      icon={<FileAddOutlined />}
                      onClick={() => {
                        setProductModal(true);
                        productForm.setFieldValue("brandId", record.id);
                      }}
                    />
                  </Tooltip>
                  <Popconfirm
                    title={t(
                      "MANAGEMENT.BRAND.TABLE.COLUMN.ACTIONS.DELETE.POP_CONFIRM",
                      {
                        brandId: record.id,
                      }
                    )}
                    onConfirm={() => {
                      brandDeleteMutation.mutate(record.id!);
                    }}
                  >
                    <Button danger={true} icon={<DeleteOutlined />} />
                  </Popconfirm>
                </Space>
              ),
            },
          ]}
        />
        <Modal
          open={brandModal}
          confirmLoading={brandPostMutation.isLoading}
          footer={null}
          onCancel={() => setBrandModal(false)}
          title={t("FORM.BRAND")}
        >
          <Form
            form={brandForm}
            onFinish={(values: { brandName: string }) =>
              brandPostMutation.mutate(values.brandName)
            }
            layout={"vertical"}
            disabled={brandPostMutation.isLoading}
          >
            <Form.Item
              name={"brandName"}
              label={t("FORM.BRAND.BRAND_NAME")}
              required={true}
              rules={[
                {
                  type: "string",
                  min: 1,
                  required: true,
                  message: t("FORM.BRAND.BRAND_NAME.VALIDATE_MESSAGE")!,
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item>
              <Space>
                <Button onClick={() => brandForm.resetFields()}>
                  {t("FORM.BRAND.RESET")}
                </Button>
                <Button type={"primary"} htmlType={"submit"}>
                  {t("FORM.BRAND.SUBMIT")}
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Modal>
        <Modal
          open={productModal}
          confirmLoading={productPostMutation.isLoading}
          footer={null}
          onCancel={() => setProductModal(false)}
          title={t("FORM.PRODUCT")}
        >
          <Form
            form={productForm}
            onFinish={(values: {
              brandId: string;
              productName: string;
              price: number;
              description: string;
            }) => productPostMutation.mutate(values)}
            layout={"vertical"}
            disabled={productPostMutation.isLoading}
          >
            <Form.Item
              name={"brandId"}
              required={true}
              rules={[
                {
                  type: "string",
                  min: 1,
                  required: true,
                  message: t("FORM.PRODUCT.BRAND_ID.VALIDATE_MESSAGE")!,
                },
              ]}
              hidden={true}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name={"productName"}
              label={t("FORM.PRODUCT.PRODUCT_NAME")}
              required={true}
              rules={[
                {
                  required: true,
                  type: "string",
                  min: 1,
                  message: t("FORM.PRODUCT.PRODUCT_NAME.VALIDATE_MESSAGE")!,
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name={"price"}
              label={t("FORM.PRODUCT.PRICE")}
              required={true}
              rules={[
                {
                  required: true,
                  type: "number",
                  min: 1,
                  message: t("FORM.PRODUCT.PRICE.VALIDATE_MESSAGE")!,
                },
              ]}
            >
              <InputNumber min={1} style={{ width: "100%" }} />
            </Form.Item>
            <Form.Item
              name={"description"}
              label={t("FORM.PRODUCT.DESCRIPTION")}
              required={true}
              rules={[
                {
                  required: true,
                  type: "string",
                  min: 1,
                  message: t("FORM.PRODUCT.DESCRIPTION.VALIDATE_MESSAGE")!,
                },
              ]}
            >
              <Input.TextArea />
            </Form.Item>
            <Form.Item>
              <Space>
                <Button onClick={() => productForm.resetFields()}>
                  {t("FORM.PRODUCT.RESET")}
                </Button>
                <Button type={"primary"} htmlType={"submit"}>
                  {t("FORM.PRODUCT.SUBMIT")}
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Modal>
      </Space>
    );
  };
  
  export default BrandManagementPage;
  
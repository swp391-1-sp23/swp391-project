import {
  CheckOutlined,
  DeleteOutlined,
  EditOutlined,
  PictureOutlined,
} from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Button,
  Card,
  Checkbox,
  Col,
  Form,
  Image,
  Input,
  List,
  Modal,
  Row,
  Space,
  Tag,
  Typography,
  Upload,
} from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import ENV from "src/constants/env";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useProfileStore from "src/hooks/states/useProfileStore";
import useTokenStore from "src/hooks/states/useTokenStore";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const ProfilePage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();
  const { profile, setProfile, mutation, ctx } = useProfileStore({
    onUpdate: () => {
      setIsModalOpen(false);
    },
  });
  const [addressModal, setAddressModal] = useState(false);
  const queryClient = useQueryClient();
  const addressQuery = useQuery({
    ...queryKeys.addressQueryKeys.get,
    select: ({ data: { data } }) => data,
    enabled: profile?.role === "Customer",
  });
  const [addressForm] = Form.useForm();

  const addressPostMutation = useMutation({
    mutationFn: async (params: components["schemas"]["AddAddressDto"]) => {
      const postAddress = apiRequest
        .path("/api/address")
        .method("post")
        .create();

      return await postAddress(params);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.addressQueryKeys.get._def,
      });
    },
  });
  const addressDeleteMutation = useMutation({
    mutationFn: async ({ addressId }: { addressId: string }) => {
      const deleteAddress = apiRequest
        .path("/api/address/{addressId}")
        .method("delete")
        .create();

      return await deleteAddress({ addressId });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.addressQueryKeys.get._def,
      });
    },
  });

  const { t } = useTranslation();

  const [avatarModal, setAvatarModal] = useState(false);
  const [avatarForm] = Form.useForm();
  const { token } = useTokenStore();
  const avatarMutation = useMutation({
    mutationFn: async (params: FormData) => {
      return await fetch([ENV.API_BASE_URL, "api", "avatar"].join("/"), {
        credentials: "include",
        body: params,
        method: "post",
        headers: {
          Authorization: ["Bearer", token].join(" "),
        },
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.profileQueryKeys.get._def,
      });
      setAvatarModal(false);
    },
  });
  return (
    <>
      <Row align={"middle"} justify={"center"} style={{ height: "100%" }}>
        <Col xs={22} sm={20} md={18}>
          <Card style={{ height: "100%" }}>
            <Row gutter={[16, 16]}>
              <Col xs={24} md={12}>
                <Row justify={"center"}>
                  <Space direction={"vertical"}>
                    <Image
                      alt={profile?.id}
                      src={
                        (profile?.avatar as Record<string, string>)?.["item2"]
                      }
                    />
                    <Row justify={"start"}>
                      <Button
                        type={"primary"}
                        icon={<PictureOutlined />}
                        onClick={() => setAvatarModal(true)}
                      >
                        {t("PROFILE.EDIT_AVATAR")}
                      </Button>
                    </Row>
                  </Space>
                </Row>
              </Col>
              <Col xs={24} md={12}>
                <Row justify={"center"}>
                  <Space direction={"vertical"} style={{ width: "100%" }}>
                    <Row justify={"end"}>
                      <Space>
                        {profile?.role === "Customer" && (
                          <Button
                            // type={"primary"}
                            icon={<EditOutlined />}
                            onClick={() => setAddressModal(true)}
                          >
                            {t("PROFILE.ADDRESS")}
                          </Button>
                        )}
                        <Button
                          type={"primary"}
                          icon={<EditOutlined />}
                          onClick={() => setIsModalOpen(true)}
                        >
                          {t("PROFILE.EDIT")}
                        </Button>
                      </Space>
                    </Row>
                    <Input
                      addonBefore={t("PROFILE.ID")}
                      readOnly={true}
                      value={profile?.id}
                    />
                    <Input
                      addonBefore={t("PROFILE.EMAIL")}
                      readOnly={true}
                      value={profile?.email!}
                    />
                    <Input
                      addonBefore={t("PROFILE.FIRST_NAME")}
                      readOnly={true}
                      value={profile?.firstName!}
                    />
                    <Input
                      addonBefore={t("PROFILE.LAST_NAME")}
                      readOnly={true}
                      value={profile?.lastName!}
                    />
                    <Input
                      addonBefore={t("PROFILE.PHONE")}
                      readOnly={true}
                      value={profile?.phone!}
                    />
                    <Input
                      addonBefore={t("PROFILE.ROLE")}
                      readOnly={true}
                      value={profile?.role}
                    />
                  </Space>
                </Row>
              </Col>
            </Row>
          </Card>
        </Col>
      </Row>
      <Modal
        footer={null}
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        title={t("FORM.PROFILE")}
        confirmLoading={mutation.isLoading}
      >
        <Form
          form={form}
          onFinish={(values: components["schemas"]["UpdateAccountDto"]) => {
            // console.log(values);
            setProfile(values);
          }}
          initialValues={{
            email: profile?.email,
            firstName: profile?.firstName,
            lastName: profile?.lastName,
            phone: profile?.phone,
          }}
          layout={"vertical"}
          disabled={mutation.isLoading}
        >
          <Form.Item
            name={"email"}
            label={t("FORM.PROFILE.EMAIL")}
            required={true}
            rules={[
              {
                type: "email",
                required: true,
                message: t("FORM.PROFILE.EMAIL.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name={"phone"}
            label={t("FORM.PROFILE.PHONE")}
            required={true}
            rules={[
              {
                type: "regexp",
                required: true,
                pattern: /(\+84|0)\d{9}/,
                message: t("FORM.PROFILE.PHONE.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name={"firstName"}
            label={t("FORM.PROFILE.FIRST_NAME")}
            required={true}
            rules={[
              {
                type: "string",
                required: true,
                min: 2,
                message: t("FORM.PROFILE.FIRST_NAME.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name={"lastName"}
            label={t("FORM.PROFILE.LAST_NAME")}
            required={true}
            rules={[
              {
                type: "string",
                required: true,
                min: 2,
                message: t("FORM.PROFILE.LAST_NAME.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item>
            <Space>
              <Button onClick={() => form.resetFields()}>
                {t("FORM.PROFILE.RESET")}
              </Button>
              <Button type={"primary"} htmlType={"submit"}>
                {t("FORM.PROFILE.SUBMIT")}
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
      <Modal
        open={addressModal}
        onCancel={() => setAddressModal(false)}
        title={t("PROFILE.ADDRESS")}
        onOk={() => setAddressModal(false)}
        confirmLoading={
          addressDeleteMutation.isLoading || addressPostMutation.isLoading
        }
        footer={null}
      >
        <Form
          form={addressForm}
          onFinish={(values: components["schemas"]["AddAddressDto"]) => {
            addressPostMutation.mutate(values);
          }}
          layout={"vertical"}
          disabled={addressPostMutation.isLoading}
        >
          <Space>
            <Form.Item
              label={t("FORM.ADDRESS.ADDRESS_NAME")}
              name={"addressName"}
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name={"isPrimary"}
              rules={[
                {
                  type: "boolean",
                },
              ]}
              initialValue={true}
              valuePropName={"checked"}
              label={t("FORM.ADDRESS.IS_PRIMARY")}
            >
              <Checkbox />
            </Form.Item>
          </Space>
          <Form.Item
            label={t("FORM.ADDRESS.CITY")}
            name={"city"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label={t("FORM.ADDRESS.DISTRICT")}
            name={"district"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label={t("FORM.ADDRESS.WARD")}
            name={"ward"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label={t("FORM.ADDRESS.STREET")}
            name={"street"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item>
            <Space>
              <Button onClick={() => addressForm.resetFields()}>
                {t("FORM.ADDRESS.RESET")}
              </Button>
              <Button type={"primary"} htmlType={"submit"}>
                {t("FORM.ADDRESS.SUBMIT")}
              </Button>
            </Space>
          </Form.Item>
        </Form>
        <List
          dataSource={addressQuery.data ?? []}
          renderItem={(item) => (
            <List.Item
              key={item.id}
              actions={[
                item.isPrimary && <Button icon={<CheckOutlined />} />,
                <Button
                  danger={true}
                  icon={<DeleteOutlined />}
                  onClick={() =>
                    addressDeleteMutation.mutate({ addressId: item.id! })
                  }
                />,
              ]}
            >
              <Space align={"baseline"}>
                <Typography.Title level={5}>
                  {item.addressName}
                </Typography.Title>
                <Typography.Text ellipsis={true}>
                  {[item.city, item.district, item.ward, item.street].join(
                    ", "
                  )}
                </Typography.Text>
              </Space>
            </List.Item>
          )}
        />
      </Modal>
      <Modal
        title={t("PROFILE.AVATAR")}
        onCancel={() => setAvatarModal(false)}
        onOk={() => setAvatarModal(false)}
        confirmLoading={avatarMutation.isLoading}
        footer={null}
        open={avatarModal}
      >
        <Form
          form={avatarForm}
          onFinish={(values) => {
            const formData = new FormData();
            formData.set("Avatar", values["avatar"]?.at(0).originFileObj);

            avatarMutation.mutate(formData);
          }}
          disabled={avatarMutation.isLoading}
        >
          <Form.Item
            name={"avatar"}
            valuePropName={"fileList"}
            getValueFromEvent={(e) => (Array.isArray(e) ? e : e?.fileList)}
          >
            <Upload.Dragger multiple={false} maxCount={1} name={"avatar"}>
              <Typography.Title level={3}>
                {t("FORM.AVATAR.DRAG")}
              </Typography.Title>
            </Upload.Dragger>
          </Form.Item>
          <Form.Item>
            <Space>
              <Button onClick={() => avatarForm.resetFields()}>
                {t("FORM.AVATAR.RESET")}
              </Button>
              <Button type={"primary"} htmlType={"submit"}>
                {t("FORM.ADDRESS.SUBMIT")}
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
      {/* {ctx} */}
    </>
  );
};

export default ProfilePage;

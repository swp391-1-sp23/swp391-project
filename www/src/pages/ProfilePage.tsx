import { EditOutlined, PictureOutlined } from "@ant-design/icons";
import { Button, Card, Col, Form, Image, Input, Modal, Row, Space } from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import useProfileStore from "src/hooks/states/useProfileStore";
import { components } from "src/utilities/apiSchemas";

const ProfilePage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();
  const { profile, setProfile, mutation, ctx } = useProfileStore({
    onUpdate: () => {
      setIsModalOpen(false);
    },
  });
  const { t } = useTranslation();
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
                      <Button type={"primary"} icon={<PictureOutlined />}>
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
                      <Button
                        type={"primary"}
                        icon={<EditOutlined />}
                        onClick={() => setIsModalOpen(true)}
                      >
                        {t("PROFILE.EDIT")}
                      </Button>
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
      {ctx}
    </>
  );
};

export default ProfilePage;

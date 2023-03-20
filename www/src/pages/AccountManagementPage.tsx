import { UserDeleteOutlined } from "@ant-design/icons";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useDebounce } from "ahooks";
import {
  Avatar,
  Button,
  Input,
  Popconfirm,
  Space,
  Table,
  Tag,
  Tooltip,
  Typography,
} from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useProfileStore from "src/hooks/states/useProfileStore";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const AccountManagementPage = () => {
  const [searchKey, setSearchKey] = useState("");
  const debouncedSearchKey = useDebounce(searchKey, {
    wait: 500,
  });
  const { t } = useTranslation();
  const query = useQuery({
    ...queryKeys.accountQueryKeys.get({ SearchKey: debouncedSearchKey }),
    select: ({ data: { data } }) => data,
  });
  const { profile } = useProfileStore();
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async (accountId: string) => {
      const deleteAccount = apiRequest
        .path("/api/account/{accountId}")
        .method("delete")
        .create();

      return await deleteAccount({ accountId });
    },
    onSuccess: (data) =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.accountQueryKeys.get._def,
      }),
  });

  return (
    <Space direction={"vertical"}>
      <Typography.Title level={2}>{t("MANAGEMENT.ACCOUNT")}</Typography.Title>
      <Input.Search
        allowClear={true}
        value={searchKey}
        onChange={({ currentTarget: { value } }) => {
          setSearchKey(value);
        }}
      />
      <Table<components["schemas"]["AccountDto"] & { actions: null }>
        dataSource={
          query.data?.map((item) => ({
            ...item,
            actions: null,
            key: item.id!,
          })) ?? []
        }
        columns={[
          {
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.ID"),
            dataIndex: "id",
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
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.EMAIL"),
            dataIndex: "email",
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
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.AVATAR"),
            dataIndex: ["avatar", "item2"],
            render: (value) => <Avatar src={value} />,
          },
          {
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.FIRST_NAME"),
            dataIndex: "firstName",
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
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.LAST_NAME"),
            dataIndex: "lastName",
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
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.PHONE"),
            dataIndex: "phone",
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
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.ROLE"),
            dataIndex: "role",
            render: (value) => (
              <Tag color={value === "Shop" ? "red" : "blue"}>{value}</Tag>
            ),
          },
          {
            title: t("MANAGEMENT.ACCOUNT.TABLE.COLUMN.ACTIONS"),
            dataIndex: "actions",
            render: (value, record, idx) => (
              <Space>
                <Popconfirm
                  title={t(
                    "MANAGEMENT.ACCOUNT.TABLE.COLUMN.ACTIONS.DELETE.POP_COMFIRM",
                    {
                      accountId: record.id,
                    }
                  )}
                  onConfirm={() => mutation.mutate(record.id!)}
                  disabled={profile?.id === record.id || record.role === "Shop"}
                >
                  <Button
                    danger={true}
                    icon={<UserDeleteOutlined />}
                    disabled={
                      profile?.id === record.id || record.role === "Shop"
                    }
                  />
                </Popconfirm>
              </Space>
            ),
          },
        ]}
      />
    </Space>
  );
};

export default AccountManagementPage;

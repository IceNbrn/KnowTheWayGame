using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TMPro;
using UnityEngine;

public class UiDatabase : MonoBehaviour
{
    public struct PlayerData
    {
        private string username;
        private string password;
        private int nationalityId;
        private int id;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int NationalityID
        {
            get { return nationalityId; }
            set { nationalityId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public override string ToString()
        {
            return $"Username: {username}, Password: {password}, NationalityID: {nationalityId}";
        }
    }

    public TMP_Dropdown DrpDownNacionality;
    public GameObject PanelFindPlayer, PanelLogin;
    public TextMeshProUGUI TextInfo, PlayerName;
    public TextMeshProUGUI m_PlayerId;

    private PlayerData m_PlayerData;

    private void Start()
    {
        m_PlayerData = new PlayerData();

        LoadDropDownNationality();
    }

    public void PlayerSetName(TMP_InputField friendName)
    {
        if (friendName == null || friendName.text == String.Empty) return;
        m_PlayerData.Username = friendName.text;
    }
    public void PlayerSetPassword(TMP_InputField password)
    {
        if (password == null || password.text == String.Empty) return;
        m_PlayerData.Password = password.text;
    }

    public void RegisterPlayer()
    {
        Debug.Log(m_PlayerData);

        string sql = "INSERT INTO Player(AvatarID, NacionalityID, Username, Password) VALUES (1, 1, @username, @password)";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter() {ParameterName="@username",SqlDbType=SqlDbType.VarChar,Value = m_PlayerData.Username},
            new SqlParameter() {ParameterName="@password",SqlDbType=SqlDbType.VarChar,Value = m_PlayerData.Password}
        };
        Database.Instance.ExecuteCmd(sql, parameters);
    }

    public void Login()
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter() {ParameterName="@username",SqlDbType=SqlDbType.VarChar,Value = m_PlayerData.Username},
            new SqlParameter() {ParameterName="@password",SqlDbType=SqlDbType.VarChar,Value = m_PlayerData.Password}
        };
        DataTable data = Database.Instance.ReturnQuery("SELECT * FROM Player WHERE Username = @username AND Password = @password", parameters);

        if (data != null && data.Rows.Count > 0)
        {
            PanelLogin.SetActive(false);
            PanelFindPlayer.SetActive(true);
            m_PlayerData.Id = int.Parse(data.Rows[0][0].ToString());
        }
        else
        {
            TextInfo.color = Color.red;
            TextInfo.SetText("Login failed, username or password doesn't exist!");
            //TextInfo.color = Color.white;
        }
    }

    private void LoadDropDownNationality()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        DataTable data = Database.Instance.ReturnQuery("SELECT * FROM Nacionality");

        foreach (DataRow row in data.Rows)
        {
            options.Add(new TMP_Dropdown.OptionData(row[1].ToString()));
        }

        DrpDownNacionality.AddOptions(options);
    }

    public void EditUsername(TMP_InputField username)
    {
        string sql = "UPDATE Player SET Username = @username WHERE PlayerID = @id";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter() {ParameterName="@username",SqlDbType=SqlDbType.VarChar,Value = username.text},
            new SqlParameter() {ParameterName="@id",SqlDbType=SqlDbType.Int,Value = m_PlayerData.Id}
        };

        Database.Instance.ExecuteCmd(sql, parameters);

    }
    public void DeletePlayer()
    {
        string sql = "DELETE FROM Player WHERE PlayerID = @id";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter() {ParameterName="@id",SqlDbType=SqlDbType.Int,Value = m_PlayerData.Id}
        };

        Database.Instance.ExecuteCmd(sql, parameters);

        m_PlayerData.Id = 0;
    }
}

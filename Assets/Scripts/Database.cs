using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public class Database : MonoBehaviour
{
    private static Database _instance;

    public string Server, DatabaseTxt, Username, Password;
    public static Database Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }
    }
    private string _connectionString;
    private SqlConnection _dbConnection;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        _instance = this;

        string server = Server;
        string database = DatabaseTxt;
        string username = Username;
        string password = Password;
        try
        {
            _connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
            _dbConnection = new SqlConnection(_connectionString);
            _dbConnection.Open();
            Debug.Log("[Database]: New database connection!");
        }
        catch (SqlException e)
        {
            Debug.Log("[Database]: " + e.Message);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void ReOpenConnection()
    {
        if (_dbConnection.State == ConnectionState.Closed)
        {
            Debug.Log("[Database]: Connection State closed!");
            Debug.Log("[Database]: Disposing...");
            _dbConnection.Dispose();
            Debug.Log("[Database]: Disposed!");
            Debug.Log("[Database]: Opening connection....");
            _dbConnection = new SqlConnection(_connectionString);
            _dbConnection.Open();
            Debug.Log("[Database]: Connection State open!");
        }
    }

    public bool ExecuteCmd(string sql)
    {
        try
        {
            //ReOpenConnection();
            SqlCommand cmd = new SqlCommand(sql, _dbConnection);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError("[Database]: " + e.Message);
            return false;
        }

        return true;
    }

    public bool ExecuteCmd(string sql, List<SqlParameter> parameters)
    {
        try
        {
            //ReOpenConnection();
            SqlCommand cmd = new SqlCommand(sql, _dbConnection);
            cmd.Parameters.AddRange(parameters.ToArray());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError("[Database]: " + e.Message);
            return false;
        }

        return true;
    }

    public bool ExecuteCmd(string sql, List<SqlParameter> parameters, SqlTransaction transaction)
    {
        try
        {
            //ReOpenConnection();
            SqlCommand cmd = new SqlCommand(sql, _dbConnection);
            cmd.Parameters.AddRange(parameters.ToArray());
            cmd.Transaction = transaction;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError("[Database]: " + e.Message);
            return false;
        }

        return true;
    }

    public DataTable ReturnQuery(string sql)
    {
        //ReOpenConnection();
        SqlCommand cmd = new SqlCommand(sql, _dbConnection);
        DataTable table = new DataTable();
        SqlDataReader dados = cmd.ExecuteReader();
        table.Load(dados);
        table.Dispose();
        table.Dispose();
        return table;
    }

    public DataTable ReturnQuery(string sql, List<SqlParameter> parameters)
    {
        //ReOpenConnection();
        SqlCommand cmd = new SqlCommand(sql, _dbConnection);
        DataTable table = new DataTable();
        cmd.Parameters.AddRange(parameters.ToArray());
        SqlDataReader dados = cmd.ExecuteReader();
        table.Load(dados);
        table.Dispose();
        cmd.Dispose();
        return table;
    }


    public DataTable ReturnQuery(string sql, List<SqlParameter> parameters, SqlTransaction transaction)
    {
        //ReOpenConnection();
        SqlCommand cmd = new SqlCommand(sql, _dbConnection);
        cmd.Transaction = transaction;
        DataTable table = new DataTable();
        cmd.Parameters.AddRange(parameters.ToArray());
        SqlDataReader dados = cmd.ExecuteReader();
        table.Load(dados);
        table.Dispose();
        cmd.Dispose();
        return table;
    }
}

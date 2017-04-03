Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports bv.common.db.Core
Imports System.ComponentModel
Imports bv.common.Objects.DBService
Imports System.Threading
Imports bv.common.Core
Imports bv.model.Model.Core
Imports bv.common.Configuration

''' -----------------------------------------------------------------------------
''' <summary>
''' <i>BaseDbService</i> class is supposed to be used as middle tier between the client and database.
''' It should hide from client any database specific operations and work as the bridge that fills dataset with data, 
''' pass it to the client application and then receive modified dataset from client and save data to the database.
''' It should also provide all needed business logic during retrieving/saving data and needed methods 
''' for any custom database related action that should be performed in the client application.
''' </summary>
''' <remarks>
''' All<i>BaseForm</i> descendant classes use overridden versions of <i>BaseDbService</i> class for 
''' interaction with database.
''' <b>Note:</b> current version of <i>BaseDbService</i> use provides the interface for interaction SQL Server database only.
''' Future versions can contain interface to any other desired database.
''' </remarks>
''' <history>
''' 	[Mike]	04.04.2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class BaseDbService
    Implements IDisposable

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used by <see cref="BaseDbService.LastError"/> property to get last error occurred inside <i>BaseDbService</i> methods.
    ''' </summary>
    ''' <remarks>
    ''' Initialize <i>m_Error</i> in the descendant classes to set the last error occurred inside the class instance.
    ''' Once initialized it can be always accessed through <see cref="BaseDbService.LastError"/> property.
    ''' </remarks>
    ''' <example>
    ''' The simplest implementation of <see cref="BaseDbService.GetDetail"/> method demonstrates how to fill dataset with 
    ''' object data and initialize protected <i>m_Error</i> and <i>m_ID</i> fields. It is assumed that
    ''' the class descendant from <i>BaseDbService</i> works with the object named <c>MyObjectName</c> 
    ''' and uses <c>spMyObjectName_SelectDetail</c> stored procedure to retrieve object data
    ''' <code>
    '''Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
    '''    Dim ds As New DataSet
    '''    Try
    '''        'Create command related with the stored procedure spMyObjectName_SelectDetail
    '''        Dim cmd As IDbCommand = CreateSPCommand("spMyObjectName_SelectDetail")
    '''        AddParam(cmd, "@ID", ID)
    '''        'Create the data adapter based on the this command
    '''        Dim adapter As DbDataAdapter = CreateAdapter(cmd)
    '''        'and fill dataset with object data
    '''        adapter.Fill(ds, "MyObjectName")
    '''        'Process the new object creation
    '''        'It is assumed that if ID is nothing we should return 
    '''        'the dataset containing empty row related with the main obiect
    '''        If ID Is Nothing Then
    '''            Dim r As DataRow = ds.Tables("MyObjectName").NewRow()
    '''            ID = Guid.NewGuid
    '''            r("ID") = ID
    '''            ds.EnforceConstraints = False
    '''            ds.Tables("MyObjectName").Rows.Add(r)
    '''        End If
    '''        'initialize  m_ID to link the class instance with specific primary key
    '''        m_ID = ID
    '''        Return ds
    '''    Catch ex As Exception
    '''        'Set the last error pointer to the current error
    '''        m_Error = New ErrorMessage(StandardError.FillDatasetError, ex)
    '''        Return Nothing
    '''    End Try
    '''    Return Nothing
    '''End Function
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	18.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected m_Error As ErrorMessage
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used by <see cref="BaseDbService.ID"/> property to get primary key of object retrieved using <see cref="BaseDbService.GetDetail"/> method.
    ''' </summary>
    ''' <remarks>
    ''' Initialize <i>m_ID</i> in the overridden <see cref="BaseDbService.GetDetail"/> method of descendant classes to set the primary key of object related with the class instance.
    ''' Once initialized it can be always accessed through <see cref="BaseDbService.ID"/> property.
    ''' </remarks>
    ''' <example>
    ''' The simplest implementation of <see cref="BaseDbService.GetDetail"/> method demonstrates how to fill dataset with 
    ''' object data and initialize protected <i>m_Error</i> and <i>m_ID</i> fields. It is assumed that
    ''' the class descendant from <i>BaseDbService</i> works with the object named <c>MyObjectName</c> 
    ''' and uses <c>spMyObjectName_SelectDetail</c> stored procedure to retrieve object data
    ''' <code>
    '''Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
    '''    Dim ds As New DataSet
    '''    Try
    '''        'Create command related with the stored procedure spMyObjectName_SelectDetail
    '''        Dim cmd As IDbCommand = CreateSPCommand("spMyObjectName_SelectDetail")
    '''        AddParam(cmd, "@ID", ID)
    '''        'Create the data adapter based on the this command
    '''        Dim adapter As DbDataAdapter = CreateAdapter(cmd)
    '''        'and fill dataset with object data
    '''        adapter.Fill(ds, "MyObjectName")
    '''        'Process the new object creation
    '''        'It is assumed that if ID is nothing we should return 
    '''        'the dataset containing empty row related with the main obiect
    '''        If ID Is Nothing Then
    '''            Dim r As DataRow = ds.Tables("MyObjectName").NewRow()
    '''            ID = Guid.NewGuid
    '''            r("ID") = ID
    '''            ds.EnforceConstraints = False
    '''            ds.Tables("MyObjectName").Rows.Add(r)
    '''        End If
    '''        'initialize  m_ID to link the class instance with specific primary key
    '''        m_ID = ID
    '''        Return ds
    '''    Catch ex As Exception
    '''        'Set the last error pointer to the current error
    '''        m_Error = New ErrorMessage(StandardError.FillDatasetError, ex)
    '''        Return Nothing
    '''    End Try
    '''    Return Nothing
    '''End Function
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	18.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected m_ID As Object = Nothing
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used by <see cref="BaseDbService.IsNewObject"/> property to indicate the state of object retrieved using <see cref="BaseDbService.GetDetail"/> method.
    ''' </summary>
    ''' <remarks>
    ''' Initialize this field inside overridden <see cref="BaseDbService.GetDetail"/> method if you intended to use <see cref="BaseDbService.IsNewObject"/> property
    ''' of descendant <i>BaseDbService</i> class.
    ''' </remarks>
    ''' <example>
    ''' The simplest implementation of <see cref="BaseDbService.GetDetail"/> method demonstrates how to fill dataset with 
    ''' object data and initialize protected <i>m_Error</i> and <i>m_ID</i> fields. It is assumed that
    ''' the class descendant from <i>BaseDbService</i> works with the object named <c>MyObjectName</c> 
    ''' and uses <c>spMyObjectName_SelectDetail</c> stored procedure to retrieve object data
    ''' <code>
    '''Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
    '''    Dim ds As New DataSet
    '''    Try
    '''        'Create command related with the stored procedure spMyObjectName_SelectDetail
    '''        Dim cmd As IDbCommand = CreateSPCommand("spMyObjectName_SelectDetail")
    '''        AddParam(cmd, "@ID", ID)
    '''        'Create the data adapter based on the this command
    '''        Dim adapter As DbDataAdapter = CreateAdapter(cmd)
    '''        'and fill dataset with object data
    '''        adapter.Fill(ds, "MyObjectName")
    '''        'Process the new object creation
    '''        'It is assumed that if ID is nothing we should return 
    '''        'the dataset containing empty row related with the main obiect
    '''        If ID Is Nothing Then
    '''            Dim r As DataRow = ds.Tables("MyObjectName").NewRow()
    '''            ID = Guid.NewGuid
    '''            r("ID") = ID
    '''            ds.EnforceConstraints = False
    '''            ds.Tables("MyObjectName").Rows.Add(r)
    '''            <b>m_IsNewObject = True</b> 
    '''        Else
    '''            <b>m_IsNewObject = False</b>
    '''        End If
    '''        'initialize  m_ID to link the class instance with specific primary key
    '''        m_ID = ID
    '''        Return ds
    '''    Catch ex As Exception
    '''        'Set the last error pointer to the current error
    '''        m_Error = New ErrorMessage(StandardError.FillDatasetError, ex)
    '''        Return Nothing
    '''    End Try
    '''    Return Nothing
    '''End Function
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	18.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected m_IsNewObject As Boolean = False
#If Debug = True Then
    Private Shared InstanceCounter As Integer = 0
    Private m_InstanceID As Integer
#End If

    Public Shared copyDSinPost As Boolean = True ' Special flag to cope with identity specific (PACS)


    Public Sub New()
#If Debug = True Then
        m_InstanceID = InstanceCounter
        InstanceCounter += 1
#End If
    End Sub

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            ' Free other state (managed objects).
            If Not ParentService Is Nothing Then
                ParentService.RemoveLinkedDbService(Me)
            End If
        End If
        If Not m_LinkedServices Is Nothing Then
            ' Free your own state (unmanaged objects).
            For Each service As ServiceParam In m_LinkedServices
                service.service.m_ParentService = Nothing
            Next
            m_LinkedServices.Clear()
        End If
    End Sub

    Protected Overrides Sub Finalize()
        ' This code cause exception in design time, I don't know why
        Try
            Dispose(False)
        Catch ex As Exception
            Dbg.Debug("error duirng db service disposing. DBService {0}, error: {1}", Me.GetType().Name, ex.ToString)
        End Try
    End Sub


#Region "Shared methods"

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets or sets the application connection string
    ''' </summary>
    ''' <remarks>
    ''' This is the shared property and defines the only connection string per the application.
    ''' If connection string is not set, it is initialized by the value <i>ConnectionString</i> 
    ''' node inside &lt;appSettings&gt; section of the application configuration file
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	04.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    ''' 
    Public ReadOnly Property ConnectionString() As String
        Get
            If m_ConnectionManager Is Nothing Then
                m_ConnectionManager = ConnectionManager.Create
            End If
            Return m_ConnectionManager.ConnectionString
        End Get
    End Property


    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Reinitialize current <i>BaseDbService</i> connection using current system credentials
    ' </summary>
    ' <remarks>
    ' It is assumed that connection string is formatted the next format specifiers:<br/>
    ' {0} - placeholder for the user login name<br/>
    ' {1} - placeholder for the user password<br/>
    ' </remarks>
    ' <history>
    ' 	[Mike]	06.04.2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------

    'Public Sub ResetConnection(Optional ByVal cnString As String = Nothing)
    '    For Each s As BaseDbService In m_Connections.Keys
    '        If (Not s Is Me) Then
    '            s.ReleaseConnection(False)
    '        End If
    '    Next
    '    ReleaseConnection(False)
    '    m_Connections.Clear()
    '    ConnectionManager.Instance = New ConnectionManager(cnString)
    '    CreateConnection()
    'End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Get default database connection using system connection string.
    ''' </summary>
    ''' <returns>
    ''' Returns new connection object.
    ''' </returns>
    ''' <remarks>
    ''' Internal connection object is also initialized with this newly created connection.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	06.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function CreateConnection() As IDbConnection
        m_ConnectionManager = ConnectionManager.Create()
        Return m_ConnectionManager.Connection
    End Function
    ''' -----------------------------------------------------------------------------
  ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Create or get from connection pool database cionnection with parameters specified
    ''' </summary>
    ''' <param name="server">
    ''' SQL server name
    ''' </param>
    ''' <param name="database">
    ''' Database name
    ''' </param>
    ''' <param name="user">
    ''' Optional SQL server user name.If it is not specified, the system connection 
    ''' string defined by <i>SQLUser</i> property is used.
    ''' </param>
    ''' <param name="password">
    ''' Optional SQL server user password. If it is not specified, the system connection 
    ''' string defined by <i>SQLPassword</i> property is used.
    ''' </param>
    ''' <param name="connectionString">
    ''' Optional connection string shablon for the new connection. If it is not specified, 
    ''' the system connection string defined by <i>SQLConnectionString</i> property is used.
    ''' </param>
    ''' <returns>
    ''' Returns new connection object.
    ''' </returns>
    ''' <remarks>
    ''' Internal connection object is also initialized with this newly created connection.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	06.04.2006 Created
    ''' 	[Artem] 16.09.2008 Changed
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function CreateConnection _
        ( _
            ByVal server As String, _
            ByVal database As String, _
            Optional ByVal user As String = Nothing, _
            Optional ByVal password As String = Nothing, _
            Optional ByVal connectionString As String = Nothing _
        ) As IDbConnection
        m_ConnectionManager = ConnectionManager.Create(server, database, user, password, connectionString)
        Return m_ConnectionManager.Connection
    End Function

    Public Function NewConnection _
        ( _
            ByVal server As String, _
            ByVal database As String, _
            Optional ByVal user As String = Nothing, _
            Optional ByVal password As String = Nothing, _
            Optional ByVal connectionString As String = Nothing _
        ) As IDbConnection
        m_ConnectionManager = ConnectionManager.CreateNew(server, database, user, password, connectionString)
        Return m_ConnectionManager.Connection
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes <b>IDbCommand</b> that should not return any data.
    ''' </summary>
    ''' <param name="cmd">
    ''' Instance of <b>IDbCommand</b> to execute
    ''' </param>
    ''' <param name="sqlConnection">
    ''' connection object that should be used for this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	06.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecCommand(ByVal cmd As IDbCommand, ByVal sqlConnection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As ErrorMessage
        If cmd Is Nothing Then
            Return New ErrorMessage(StandardError.InvalidParameter)
        End If
        Dim CloseConnection As Boolean = False
        If sqlConnection Is Nothing Then
            sqlConnection = ConnectionManager.DefaultInstance.Connection
        End If
        If Not Transaction Is Nothing AndAlso Not Transaction.Connection Is Nothing Then
            cmd.Transaction = Transaction
            cmd.Connection = Transaction.Connection
        Else
            cmd.Connection = sqlConnection
        End If
        SyncLock cmd.Connection
            Try
                If cmd.Connection.State <> ConnectionState.Open Then
                    cmd.Connection.Open()
                    CloseConnection = True
                End If
                cmd.ExecuteNonQuery()
                Return Nothing
            Catch e As StoredProcException
                PrintSQLCommandError(cmd, e)
                Throw
            Catch e As SqlClient.SqlException
                PrintSQLCommandError(cmd, e)
                If ThrowExceptionOnError Then
                    Throw
                End If
                Select Case e.Number
                    Case 15211
                        Return New ErrorMessage(StandardError.InvalidOldPassword)
                    Case 18456
                        Return New ErrorMessage(StandardError.InvalidLogin)
                    Case 229, 916 'this is a workaround for sp4 _bug
                        'drop SELECT permission denied on object 'sysjobs', database 'msdb', owner 'dbo'
                        Return Nothing
                    Case Else
                        PrintSQLCommandError(cmd, e)
                        If cmd.CommandType = CommandType.StoredProcedure Then
                            Return New ErrorMessage(StandardError.StoredProcedureError, e)
                        Else
                            Return New ErrorMessage(StandardError.SqlQueryError, e)
                        End If
                End Select
            Catch e As Exception
                PrintSQLCommandError(cmd, e)
                If ThrowExceptionOnError Then
                    Throw
                End If
                If cmd.CommandType = CommandType.StoredProcedure Then
                    Return New ErrorMessage(StandardError.StoredProcedureError, e)
                Else
                    Return New ErrorMessage(StandardError.SqlQueryError, e)
                End If
            Finally
                If (Transaction Is Nothing OrElse Transaction.Connection Is Nothing) AndAlso CloseConnection Then
                    cmd.Connection.Close()
                End If
            End Try
        End SyncLock
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes query defined by the <b>IDbCommand</b> object, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.
    ''' </summary>
    ''' <param name="cmd">
    ''' <b>IDbCommand</b> object containing query to execute
    ''' </param>
    ''' <param name="sqlConnection">
    ''' connection object that should be used for this command
    ''' </param>
    ''' <param name="errMsg">
    ''' returns <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' The first column of the first row in the result set, or <b>Nothing</b> if the result set is empty.
    '''</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	06.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecScalar(ByVal cmd As IDbCommand, ByVal sqlConnection As IDbConnection, ByRef errMsg As ErrorMessage, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As Object

        If cmd Is Nothing Then
            Return New ErrorMessage(StandardError.InvalidParameter)
        End If
        If sqlConnection Is Nothing Then
            sqlConnection = ConnectionManager.DefaultInstance.Connection
        End If
        If Not Transaction Is Nothing AndAlso Not Transaction.Connection Is Nothing Then
            cmd.Transaction = Transaction
            cmd.Connection = Transaction.Connection
        Else
            cmd.Connection = sqlConnection
        End If

        SyncLock cmd.Connection
            Try
                If cmd.Connection.State <> ConnectionState.Open Then
                    cmd.Connection.Open()
                End If
                Return cmd.ExecuteScalar()
            Catch e As Exception
                PrintSQLCommandError(cmd, e)
                If ThrowExceptionOnError Then
                    Throw
                End If
                If cmd.CommandType = CommandType.StoredProcedure Then
                    errMsg = New ErrorMessage(StandardError.StoredProcedureError, e)
                Else
                    errMsg = New ErrorMessage(StandardError.SqlQueryError, e)
                End If
                Return Nothing
            Finally
                If Transaction Is Nothing OrElse Transaction.Connection Is Nothing Then
                    cmd.Connection.Close()
                End If
            End Try
        End SyncLock
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates <i>IDbCommand</i> object using <i>SqlText</i> as SQL query that should be executed. 
    ''' Use this method to create <i>IDbCommand</i> instances that are independent 
    ''' from database currently used by the system.
    ''' </summary>
    ''' <param name="SqlText">
    ''' the text of SQL query for the <i>IDbCommand</i> object
    ''' </param>
    ''' <param name="Connection">
    ''' <i>IDbConnection</i> for the created command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object for the created command. If transaction 
    ''' is specified, it will be roll backed if the error during command execution occurred.
    ''' </param>
    ''' <returns>
    ''' New <i>IDbCommand</i> object
    ''' </returns>
    ''' <remarks>
    ''' In general this method can create the command of any type, but current implementation 
    ''' creates <b>SqlCommand</b> instances only.
    ''' Using this method is more preferable than using direct command constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	07.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CreateCommand(ByVal SqlText As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing) As IDbCommand
        Dim cn As SqlConnection
        If Connection IsNot Nothing Then
            cn = CType(Connection, SqlConnection)
        Else
            cn = Nothing
        End If
        If Not Transaction Is Nothing Then
            If Not Transaction.Connection Is Nothing Then
                cn = CType(Transaction.Connection, SqlConnection)
            End If
        End If
        Dim cmd As New SqlCommand(SqlText, cn, CType(Transaction, SqlTransaction))
        cmd.CommandTimeout = 300
        Return cmd
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates <i>IDbCommand</i> object using <i>SqlText</i> as name of stored procedure that should be executed. 
    ''' Use this method to create <i>IDbCommand</i> instances that are independent 
    ''' from database currently used by the system.
    ''' </summary>
    ''' <param name="SqlText">
    ''' the name of stored procedure.
    ''' </param>
    ''' <param name="Connection">
    ''' <i>IDbConnection</i> for the created command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object for the created command. If transaction 
    ''' is specified, it will be roll backed if the error during command execution occurred.
    ''' </param>
    ''' <returns>
    ''' New <i>IDbCommand</i> object
    ''' </returns>
    ''' <remarks>
    ''' In general this method can create the command of any type, but current implementation 
    ''' creates <b>SqlCommand</b> instances only.
    ''' Using this method is more preferable than using direct command constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	07.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CreateSPCommand(ByVal SqlText As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing) As IDbCommand
        Dim cn As SqlConnection = CType(Connection, SqlConnection)
        If Not Transaction Is Nothing Then
            If Not Transaction.Connection Is Nothing Then
                cn = CType(Transaction.Connection, SqlConnection)
            End If
        End If
        Dim cmd As New SqlCommand(SqlText, cn, CType(Transaction, SqlTransaction))
        cmd.CommandTimeout = 300
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function
    Public Shared Function CreateSPCommandWithParams(ByVal SqlText As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal paramValues As Dictionary(Of String, Object) = Nothing) As IDbCommand
        Dim cmd As IDbCommand = CreateSPCommand(SqlText, Connection, Transaction)
        StoredProcParamsCache.CreateParameters(cmd, paramValues)
        Return cmd
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates the instance of <b>DbDataAdapter</b> using passed command as <b>SelectCommand</b>.
    ''' </summary>
    ''' <param name="cmd">
    ''' <b>IDbCommand</b> object that will be used as <b>SelectCommand</b> for new <b>DbDataAdapter</b> 
    ''' </param>
    ''' <param name="UseCommandBuilder">
    ''' If <b>True</b>, creates <b>Insert</b>, <b>Update</b> and <b>Delete</b> commands for data adapter using passed command as the source Select command.
    ''' </param>
    ''' <returns>
    ''' new instance of <b>DbDataAdapter</b> object
    ''' </returns>
    ''' <remarks>
    ''' In general this method can create the <b>DbDataAdapter</b> of any type, but current implementation 
    ''' creates <b>SqlDataAdapter</b> instances only.
    ''' Using this method is more preferable than using direct data adapter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	07.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CreateAdapter(ByVal cmd As IDbCommand, Optional ByVal UseCommandBuilder As Boolean = False) As DbDataAdapter
        If (cmd Is Nothing) Then Return Nothing
        Dim da As New SqlDataAdapter(CType(cmd, SqlCommand))
        da.MissingSchemaAction = MissingSchemaAction.AddWithKey
        If UseCommandBuilder Then
            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand
            da.DeleteCommand = cb.GetDeleteCommand
            da.UpdateCommand = cb.GetUpdateCommand
        End If
        Return da
    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates the instance of <b>DbDataAdapter</b> using passed sql query as base for <b>SelectCommand</b>.
    ''' </summary>
    ''' <param name="SelectSql">
    ''' the sql query that should be used for creating <b>SelectCommand</b>.
    ''' </param>
    ''' <param name="Connection">
    ''' <i>IDbConnection</i> for the created <b>SelectCommand</b>
    ''' </param>
    ''' <param name="UseCommandBuilder">
    ''' If <b>True</b>, creates <b>Insert</b>, <b>Update</b> and <b>Delete</b> commands for data adapter using passed command as the source Select command.
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object for the created <b>SelectCommand</b>. If transaction 
    ''' is specified, it will be roll backed if the error during command execution occurred.
    ''' </param>
    ''' <returns>
    ''' new instance of <b>DbDataAdapter</b> object
    ''' </returns>
    ''' <remarks>
    ''' In general this method can create the <b>DbDataAdapter</b> of any type, but current implementation 
    ''' creates <b>SqlDataAdapter</b> instances only.
    ''' Using this method is more preferable than using direct data adapter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	07.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CreateAdapter(ByVal SelectSql As String, ByVal Connection As IDbConnection, Optional ByVal UseCommandBuilder As Boolean = False, Optional ByVal Transaction As IDbTransaction = Nothing) As DbDataAdapter
        Dim cmd As IDbCommand = CreateCommand(SelectSql, Connection, CType(Transaction, SqlTransaction))
        Return CreateAdapter(cmd, UseCommandBuilder)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates the instance of <b>DbDataAdapter</b> using columns of <b>DataTable</b> as source for generating <b>SelectCommand</b>.
    ''' </summary>
    ''' <param name="dt">
    ''' <b>DataTable</b> object which structure will be used as source for generating data adapter <b>SelectCommand</b>.
    ''' </param>
    ''' <param name="TableName">
    ''' the name of source table in the database
    ''' </param>
    ''' <param name="Connection">
    ''' <i>IDbConnection</i> for the created <b>SelectCommand</b>
    ''' </param>
    ''' <param name="UseCommandBuilder">
    ''' If <b>True</b>, creates <b>Insert</b>, <b>Update</b> and <b>Delete</b> commands for data adapter using passed command as the source Select command.
    ''' </param>
    ''' <param name="Transaction1">
    ''' Optional <b>IDbTransaction</b> object for the created <b>SelectCommand</b>. If transaction 
    ''' is specified, it will be roll backed if the error during command execution occurred.
    ''' </param>
    ''' <returns>
    ''' new instance of <b>DbDataAdapter</b> object
    ''' </returns>
    ''' <remarks>
    ''' This method generates Select SQL query using the names of editable <b>DataTable</b> columns to construct SELECT part of the query 
    ''' and <i>TableName</i> parameter for FROM clause. No WHERE clause is used for the generated command.
    ''' Usually this method is used with <i>UseCommandBuilder</i> parameter set to <b>True</b> to create data adapter
    ''' for posting data from passed <b>DataTable</b> into the database.
    ''' In general this method can create the <b>DbDataAdapter</b> of any type, but current implementation 
    ''' creates <b>SqlDataAdapter</b> instances only.
    ''' Using this method is more preferable than using direct data adapter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	07.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CreateAdapter(ByVal dt As DataTable, ByVal TableName As String, ByVal Connection As IDbConnection, Optional ByVal UseCommandBuilder As Boolean = False, Optional ByVal Transaction1 As IDbTransaction = Nothing) As DbDataAdapter
        Dim Fields As String = ""
        For Each col As DataColumn In dt.Columns
            If col.ReadOnly = False Then
                If Fields.Length = 0 Then
                    Fields = col.ColumnName
                Else
                    Fields += String.Format(", {0}", col.ColumnName)
                End If
            End If
        Next
        Dim sql As String = String.Format("Select {0} from {1}", Fields, TableName)
        Return CreateAdapter(sql, Connection, UseCommandBuilder, Transaction1)
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds new parameter to the <b>IDbCommand</b> parameters list
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> to which the parameter should be added
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamValue">
    ''' the parameter value
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function has no error handling inside, so exception will be thrown if the error occurs inside the function.
    ''' Use this function if you want process exceptions outside of function body. The <b>Type</b> of parameter is defined 
    ''' by the <i>ParamValue</i> object <b>Type</b>, so you should not use this function with <i>ParamValue</i>
    ''' that is set to <b>Nothing</b> or <b>DBNull.Value</b>, use SetTypedParam function instead in this case.
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	10.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamValue As Object, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        If ParamValue Is Nothing Then ParamValue = DBNull.Value
        If cmd.Parameters.Contains(ParamName) Then
            CType(cmd.Parameters(ParamName), IDataParameter).Value = ParamValue
        Else
            Dim p As New SqlParameter(ParamName, ParamValue)
            p.Direction = Direction
            cmd.Parameters.Add(p)
            'Return p
        End If
        Return cmd.Parameters(ParamName)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets the value of specific <b>IDbCommand</b> parameter
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamValue">
    ''' the parameter value
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of the parameter object.
    ''' </returns>
    ''' <remarks>
    ''' If the parameter with name <i>ParamName</i> doesn't exist it is added to the command parameters list, 
    ''' in other case the attributes of existing parameter is modified. The <b>Type</b> of parameter is defined 
    ''' by the <i>ParamValue</i> object <b>Type</b>, so you should not use this function with <i>ParamValue</i>
    ''' that is set to <b>Nothing</b> or <b>DBNull.Value</b>, use SetTypedParam function instead in this case.
    ''' In general this function can work with the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	10.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function SetParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamValue As Object, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        If ParamValue Is Nothing Then ParamValue = DBNull.Value
        Dim p As SqlParameter
        If cmd.Parameters.Contains(ParamName) Then
            p = CType(cmd.Parameters(ParamName), SqlParameter)
            p.Value = ParamValue
        Else
            p = New SqlParameter(ParamName, ParamValue)
            p.Direction = Direction
            cmd.Parameters.Add(p)
        End If
        Return p
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets the value of specific <b>IDbCommand</b> parameter to the <b>DBNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of the parameter object.
    ''' </returns>
    ''' <remarks>
    ''' If the parameter with name <i>ParamName</i> doesn't exist it is added to the command parameters list, 
    ''' in other case the value of existing parameter is set to <b>DBNull.Value</b>. 
    ''' In general this function can work with the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	10.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function SetTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As SqlParameter
        If cmd.Parameters.Contains(ParamName) Then
            p = CType(cmd.Parameters(ParamName), SqlParameter)
            p.Value = DBNull.Value
        Else
            Return AddTypedParam(cmd, ParamName, ParamType, Direction)
        End If
        Return p
    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds new parameter to the <b>IDbCommand</b> parameters list
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> to which the parameter should be added
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamValue">
    ''' the parameter value
    ''' </param>
    ''' <param name="errMsg"></param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function handles the error inside and returns the error in <i>errMsg</i> parameter. Check <i>errMsg</i> parameter
    ''' to find that error occurs inside the function. 
    ''' The <b>Type</b> of parameter is defined 
    ''' by the <i>ParamValue</i> object <b>Type</b>, so you should not use this function with <i>ParamValue</i>
    ''' that is set to <b>Nothing</b> or <b>DBNull.Value</b>, use SetTypedParam function instead in this case.
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	10.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamValue As Object, ByRef errMsg As ErrorMessage, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        Try
            Return AddParam(cmd, ParamName, ParamValue, Direction)
        Catch ex As Exception
            errMsg = New ErrorMessage(StandardError.CreateParameterError, ex)
        End Try
        Return Nothing
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added. 
    ''' There is no error handling inside this function, so exception will be thrown if the error occurs inside the function.
    ''' Use this function if you want process exceptions outside of function body. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	10.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As New SqlParameter(ParamName, ParamType)
        p.Direction = Direction
        p.Value = DBNull.Value
        p.IsNullable = True
        cmd.Parameters.Add(p)
        Return p
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="strSrcColumn">
    ''' the column in the related <b>DataTable</b> to which this parameter should be linked
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added and you want to link the parameter with the specific column in the related <b>DataTable</b>. 
    ''' There is no error handling inside this function, so exception will be thrown if the error occurs inside the function.
    ''' Use this function if you want process exceptions outside of function body. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByVal strSrcColumn As String, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As New SqlParameter(ParamName, ParamType)
        p.Direction = Direction
        p.Value = DBNull.Value
        p.IsNullable = True
        p.SourceColumn = strSrcColumn
        cmd.Parameters.Add(p)
        Return p
    End Function

    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByVal Size As Integer, ByVal strSrcColumn As String, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As New SqlParameter(ParamName, ParamType, Size)
        p.Direction = Direction
        p.Value = DBNull.Value
        p.IsNullable = True
        p.SourceColumn = strSrcColumn
        cmd.Parameters.Add(p)
        Return p
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="strSrcColumn">
    ''' the column in the related <b>DataTable</b> to which this parameter should be linked
    ''' </param>
    ''' <param name="SourceType">
    ''' the source of parameter like current, original, etc
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added and you want to link the parameter with the specific column in the related <b>DataTable</b>. 
    ''' There is no error handling inside this function, so exception will be thrown if the error occurs inside the function.
    ''' Use this function if you want process exceptions outside of function body. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Andrey]	09.03.2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByVal strSrcColumn As String, ByVal SourceType As DataRowVersion, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As New SqlParameter(ParamName, ParamType)
        p.Direction = Direction
        p.Value = DBNull.Value
        p.IsNullable = True
        p.SourceColumn = strSrcColumn
        p.SourceVersion = SourceType
        cmd.Parameters.Add(p)
        Return p
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b> returning <i>ErrorMessage</i> if error occurred.
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="errMsg">
    ''' returns <i>ErrorMessage</i> object if error occur or <b>Nothing</b> in other case.
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByRef errMsg As ErrorMessage, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        Try
            Return AddTypedParam(cmd, ParamName, ParamType, Direction)
        Catch ex As Exception
            errMsg = New ErrorMessage(StandardError.CreateParameterError, ex)
        End Try
        Return Nothing
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type and size to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="aSize">
    ''' size of the created parameter
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added and not default size for this parameter type should be used. 
    ''' There is no error handling inside this function, so exception will be thrown if the error occurs inside the function.
    ''' Use this function if you want process exceptions outside of function body. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByVal aSize As Integer, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        If cmd Is Nothing Then Return Nothing
        Dim p As New SqlParameter(ParamName, ParamType)
        p.Direction = Direction
        p.Value = DBNull.Value
        p.Size = aSize
        cmd.Parameters.Add(p)
        Return p
    End Function

    ''' -----------------------------------------------------------------------------
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds the new parameter of specific type and size to the <b>IDbCommand</b> object and sets it to the <b>DbNull.Value</b>
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be modified
    ''' </param>
    ''' <param name="ParamName">
    ''' the parameter name
    ''' </param>
    ''' <param name="ParamType">
    ''' the <b>SqlDbType</b> of the parameter
    ''' </param>
    ''' <param name="aSize">
    ''' size of the created parameter
    ''' </param>
    ''' <param name="errMsg">
    ''' returns <i>ErrorMessage</i> object if error occur or <b>Nothing</b> in other case.
    ''' </param>
    ''' <param name="Direction">
    ''' the parameter direction
    ''' </param>
    ''' <returns>
    ''' Returns instance of created parameter object.
    ''' </returns>
    ''' <remarks>
    ''' This function should be called if new parameter without defined value should be added and not default size for this parameter type should be used. 
    ''' In general this function can create the parameter object of any type, but current implementation 
    ''' creates <b>SqlParameter</b> instances only.
    ''' Using this function is more preferable than using direct parameter constructor call because this
    ''' allows concentrating all database specific codes in one place.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddTypedParam(ByVal cmd As IDbCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, ByVal aSize As Integer, ByRef errMsg As ErrorMessage, Optional ByVal Direction As ParameterDirection = ParameterDirection.Input) As Object
        Try
            Return AddTypedParam(cmd, ParamName, ParamType, aSize, Direction)
        Catch ex As Exception
            errMsg = New ErrorMessage(StandardError.CreateParameterError, ex)
        End Try
        Return Nothing
    End Function



    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns the of specified parameter from <b>IDbCommand</b> parameters list
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter should be retrieved
    ''' </param>
    ''' <param name="ParamName">
    ''' the name of parameter to return
    ''' </param>
    ''' <returns>
    ''' returns the parameter defined by <i>ParamName</i>.
    ''' </returns>
    ''' <remarks>
    ''' No error processing is performed inside this method. If you pass the <i>ParamName</i> 
    ''' that points to the absent parameter the exception will be thrown.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetParam(ByVal cmd As IDbCommand, ByVal ParamName As String) As Object
        If cmd Is Nothing Then Return Nothing
        Return cmd.Parameters(ParamName)
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns the value stored in the specific parameter of <i>IDbCommand</i> object.
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter value should be retrieved
    ''' </param>
    ''' <param name="ParamName">
    ''' the name of parameter to return
    ''' </param>
    ''' <returns>
    ''' returns the value of parameter defined by <i>ParamName</i>.
    ''' </returns>
    ''' <remarks>
    ''' No error processing is performed inside this method. If you pass the <i>ParamName</i> 
    ''' that points to the absent parameter the exception will be thrown.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetParamValue(ByVal cmd As IDbCommand, ByVal ParamName As String) As Object
        If cmd Is Nothing Then Return Nothing
        Return CType(cmd.Parameters(ParamName), SqlParameter).Value
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes SQL statement that should not return any data.
    ''' </summary>
    ''' <param name="SQL">
    ''' SQL statement that should be executed
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecSql(ByVal SQL As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As ErrorMessage
        Dim cmd As IDbCommand = CreateCommand(SQL, Connection)
        Return ExecCommand(cmd, Connection, Transaction, ThrowExceptionOnError)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes stored procedure against the Connection object of a .NET Framework data provider.
    ''' </summary>
    ''' <param name="ProcName">
    ''' the name of stored procedure to execute
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for executing this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecProcedure(ByVal ProcName As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As ErrorMessage
        Dim cmd As IDbCommand = CreateSPCommand(ProcName, Connection)
        Return ExecCommand(cmd, Connection, Transaction, ThrowExceptionOnError)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes the query, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored
    ''' </summary>
    ''' <param name="SQL">
    ''' the SQL statement that should be executed
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for executing this command
    ''' </param>
    ''' <param name="errMsg">
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' Returns the first column of the first row in the result set or <b>Nothing</b> if error occurred .
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecScalar(ByVal SQL As String, ByVal Connection As IDbConnection, ByRef errMsg As ErrorMessage, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As Object
        Dim cmd As IDbCommand = CreateCommand(SQL, Connection)
        Return ExecScalar(cmd, Connection, errMsg, Transaction, ThrowExceptionOnError)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes <b>IDbCommand</b> that should not return any data and using the set of data stored in the <b>Hashtable</b> as command parameters.
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> which parameter should be retrieved
    ''' </param>
    ''' <param name="Params">
    ''' the set of data that should be used as parameters for the command. The parameter name should be used as key of the <b>Hashtable</b> object, 
    ''' and the parameter values should be stored as <b>Hashtable</b> values.
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for executing this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function ExecCommand(ByVal cmd As IDbCommand, ByVal Params As Hashtable, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal ThrowExceptionOnError As Boolean = False) As ErrorMessage
        If cmd Is Nothing Then
            Return New ErrorMessage(StandardError.InvalidParameter)
        End If
        Dim errMsg As ErrorMessage = Nothing
        If Not Params Is Nothing Then
            For Each key As String In Params.Keys
                AddParam(cmd, key, Params(key), errMsg)
                If Not errMsg Is Nothing Then Return errMsg
            Next
        End If
        Return ExecCommand(cmd, Connection, Transaction, ThrowExceptionOnError)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes SQL statement against the Connection object of a .NET Framework data provider 
    ''' and returns the results inside <b>DataTable</b> object.
    ''' </summary>
    ''' <param name="queryText">
    ''' SQL statement that should be executed
    ''' </param>
    ''' <param name="dt">
    ''' <b>DataTable</b> object that will be filled by returned data
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for executing this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' The data containing in the <b>DataTable</b> are not cleared but appended with the new one.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FillTable(ByVal queryText As String, ByVal dt As DataTable, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal SkipDebugOutput As Boolean = False) As ErrorMessage
        If Not SkipDebugOutput Then
            DebugTimer.Start(String.Format("FillTable call, query={0}", queryText))
        End If
        Try
            Dim da As DbDataAdapter = CreateAdapter(queryText, Connection, False, Transaction)
            da.Fill(dt)
            Return Nothing
        Catch ex As Exception
            Return New ErrorMessage(StandardError.FillDatasetError, ex)
        Finally
            If Not SkipDebugOutput Then
                DebugTimer.Stop()
            End If
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes command against the Connection object of a .NET Framework data provider 
    ''' and returns the results inside <b>DataTable</b> object.
    ''' </summary>
    ''' <param name="cmd">
    ''' instance of <b>IDbCommand</b> that should be executed
    ''' </param>
    ''' <param name="dt">
    ''' <b>DataTable</b> object that will be filled by returned data
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' The data containing in the <b>DataTable</b> are not cleared but appended with the new one.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FillTable(ByVal cmd As IDbCommand, ByVal dt As DataTable, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal SkipDebugOutput As Boolean = False) As ErrorMessage
        If Not SkipDebugOutput Then
            DebugTimer.Start(String.Format("FillTable call, commandText={0}", cmd.CommandText))
        End If
        Try
            cmd.Transaction = Transaction
            Dim da As DbDataAdapter = CreateAdapter(cmd, False)
            da.Fill(dt)
            Return Nothing
        Catch ex As Exception
            Return New ErrorMessage(StandardError.FillDatasetError, ex)
        Finally
            If Not SkipDebugOutput Then
                DebugTimer.Stop()
            End If
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Executes SQL statement against the Connection object of a .NET Framework data provider 
    ''' and returns the results inside specific table of <b>DataSet</b> object.
    ''' </summary>
    ''' <param name="queryText">
    ''' SQL statement that should be executed
    ''' </param>
    ''' <param name="ds">
    ''' <b>DataSet</b> object that will contain returned results
    ''' </param>
    ''' <param name="TableName">
    ''' the name of table in the <b>DataSet.Tables</b> collection that will contain returned results
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for executing this command
    ''' </param>
    ''' <param name="Transaction">
    ''' Optional <b>IDbTransaction</b> object. If it is passed, the command is executed as part of this 
    ''' transaction and entire transaction will be roll backed if execution fails.
    ''' </param>
    ''' <returns>
    ''' <i>ErrorMessage</i> object if error occurs during command execution or <b>Nothing</b> if command executed successfully.
    ''' </returns>
    ''' <remarks>
    ''' The data containing in the dataset table are not cleared but appended with the new one. If table with passed name doesn't not exist in the <b>DataSet</b>, it is added to the dataset's <b>Tables</b> collection.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FillDataset(ByVal queryText As String, ByVal ds As DataSet, ByVal TableName As String, ByVal Connection As IDbConnection, Optional ByVal Transaction As IDbTransaction = Nothing) As ErrorMessage
        Try
            Dim da As DbDataAdapter = CreateAdapter(queryText, Connection, False, Transaction)
            da.Fill(ds, TableName)
            Return Nothing
        Catch ex As Exception
            Return New ErrorMessage(StandardError.FillDatasetError, ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets the transaction for each command in the <b>DbDataAdapter</b> object.
    ''' </summary>
    ''' <param name="da">
    ''' <b>DbDataAdapter</b> object to which the transaction should be applied
    ''' </param>
    ''' <param name="transaction">
    ''' <b>IDbTransaction</b> object that should be applied to the data adapter commands
    ''' </param>
    ''' <remarks>
    ''' The transaction is applied to each defined command in the data adapter.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Sub ApplyTransaction(ByVal da As DbDataAdapter, ByVal transaction As IDbTransaction)

        Dim ida As IDbDataAdapter = CType(da, IDbDataAdapter)
        If (Not ida.SelectCommand Is Nothing) Then
            ida.SelectCommand.Transaction = transaction
        End If
        If (Not ida.InsertCommand Is Nothing) Then
            ida.InsertCommand.Transaction = transaction
        End If
        If (Not ida.UpdateCommand Is Nothing) Then
            ida.UpdateCommand.Transaction = transaction
        End If
        If (Not ida.DeleteCommand Is Nothing) Then
            ida.DeleteCommand.Transaction = transaction
        End If
        If Not transaction Is Nothing Then
            If (Not ida.SelectCommand Is Nothing) Then
                ida.SelectCommand.Connection = transaction.Connection
            End If
            If (Not ida.InsertCommand Is Nothing) Then
                ida.InsertCommand.Connection = transaction.Connection
            End If
            If (Not ida.UpdateCommand Is Nothing) Then
                ida.UpdateCommand.Connection = transaction.Connection
            End If
            If (Not ida.DeleteCommand Is Nothing) Then
                ida.DeleteCommand.Connection = transaction.Connection
            End If

        End If

    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Posts changes stored in the <b>DataSet</b> to the database using passed <b>DbDataAdapter</b>
    ''' </summary>
    ''' <param name="da">
    ''' <b>DbDataAdapter</b> object that should perform data posting
    ''' </param>
    ''' <param name="ds">
    ''' <b>DataSet</b> object that contains data to post
    ''' </param>
    ''' <param name="TableName">
    ''' the name of <b>DataTable</b> in the <b>DataSet.Tables</b> collection, which data should be posted to the database
    ''' </param>
    ''' <param name="transaction">
    ''' The <b>IDbTransaction</b> object that should be applied to each defined command in the data adapter.
    ''' </param>
    ''' <returns>
    '''The number of rows successfully updated from the <b>DataSet</b>.
    ''' </returns>
    ''' <remarks>
    ''' No exceptions are processed inside this method. You should process all exceptions in the codes that use this method.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	12.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function Update(ByVal da As DbDataAdapter, ByVal ds As DataSet, ByVal TableName As String, Optional ByVal transaction As IDbTransaction = Nothing) As Integer
        If da Is Nothing OrElse ds Is Nothing OrElse TableName Is Nothing Then Return -1
        ApplyTransaction(da, transaction)
        Return da.Update(ds, TableName)
    End Function
    Public Shared Function Update(ByVal da As DbDataAdapter, ByVal table As DataTable, Optional ByVal DeleteSort As String = Nothing, Optional ByVal transaction As IDbTransaction = Nothing, Optional filter As String = "", Optional rowUpdated As SqlRowUpdatedEventHandler = Nothing) As Integer
        If da Is Nothing OrElse table Is Nothing Then Return -1
        ApplyTransaction(da, transaction)
        If Not rowUpdated Is Nothing Then
            AddHandler CType(da, SqlDataAdapter).RowUpdated, rowUpdated
        End If
        Dim result As Integer = 0
        Dim updateType As Integer = 0
        Try
            Dim rows As DataRow() = table.Select(filter, DeleteSort, DataViewRowState.Deleted)
            result += da.Update(rows)
            updateType += 1
            rows = table.Select(filter, "", DataViewRowState.ModifiedCurrent)
            result += da.Update(rows)
            updateType += 1
            rows = table.Select(filter, "", DataViewRowState.Added)
            result += da.Update(rows)
            Return result 'da.Update(table)
        Catch ex As Exception
            Select Case updateType
                Case 0
                    PrintSQLCommandError(da.DeleteCommand, ex)
                Case 1
                    PrintSQLCommandError(da.UpdateCommand, ex)
                Case 2
                    PrintSQLCommandError(da.InsertCommand, ex)
            End Select
            Throw
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Searches the <b>DataRow</b> in the <b>DataTable</b> by the value of <b>DataRow</b> key field
    ''' </summary>
    ''' <param name="table">
    '''  <b>DataTable</b> object to search
    ''' </param>
    ''' <param name="aKey">
    ''' the value of the key field.
    ''' </param>
    ''' <param name="keyFieldName">
    ''' the name of the key field. If not specified the table primary key field is used as key field.
    ''' </param>
    ''' <returns>
    ''' Returns the <b>DataRow</b> object, or nothing if there is no <b>DataRow</b> with such key.
    ''' </returns>
    ''' <remarks>
    ''' It is assumed that <b>DataTable</b> has the only primary key field. 
    ''' If <i>keyFieldName</i> parameter is specified the function returns the first row that matches the search criteria.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FindRow(ByVal table As DataTable, ByVal aKey As Object, Optional ByVal keyFieldName As String = Nothing) As DataRow
        If (table Is Nothing) Or (aKey Is Nothing) Then Return Nothing
        Try
            If keyFieldName Is Nothing Then
                Return table.Rows.Find(aKey)
            Else
                If table.Columns.Contains(keyFieldName) = False Then
                    Return Nothing
                End If
                For Each row As DataRow In table.Rows
                    If row.RowState <> DataRowState.Deleted AndAlso row(keyFieldName).Equals(aKey) Then
                        Return row
                    End If
                Next
            End If
        Catch
            Return Nothing
        End Try
        Return Nothing
    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets the value of specific field in the first row of the <b>DataTable</b>. 
    ''' If <b>DataTable</b> is empty the new row is added to the <b>DataTable</b> firstly
    ''' </summary>
    ''' <param name="dt">
    ''' <b>DataTable</b> object that will be processed 
    ''' </param>
    ''' <param name="FieldName">
    ''' the name of the field that should be modified
    ''' </param>
    ''' <param name="Val">
    ''' the new field value
    ''' </param>
    ''' <returns>
    ''' Returns the <b>DataRow</b> object that was modified.
    ''' </returns>
    ''' <remarks>
    ''' The function modified the first row of the <b>DataTable</b> object and can be 
    ''' used in the detail forms for modification of the main detail form object that is always related 
    ''' with the first and only <b>DataRow</b> in the <b>DataTable</b>.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function SetDetailTableValue(ByVal dt As DataTable, ByVal FieldName As String, ByVal Val As Object) As DataRow
        If dt Is Nothing Then Return Nothing
        Dim r As DataRow = Nothing
        For Each row As DataRow In dt.Rows
            If row.RowState <> DataRowState.Deleted AndAlso row.RowState <> DataRowState.Detached Then
                r = row
                Exit For
            End If
        Next
        If r Is Nothing Then
            r = dt.NewRow
            dt.Rows.Add(r)
        End If
        If Val Is Nothing Then Val = DBNull.Value
        r(FieldName) = Val
        Return r
    End Function

    Public Shared Sub DeleteRowWithAccept(ByVal row As DataRow)
        If row.RowState = DataRowState.Added Then
            row.Delete()
        Else
            row.BeginEdit()
            row.Delete()
            row.AcceptChanges()
            row.EndEdit()
        End If

    End Sub
    Public Shared Sub DeleteTableRows(ByVal t As DataTable)
        For i As Integer = t.Rows.Count - 1 To 0 Step -1
            t.Rows(i).Delete()
        Next
    End Sub

#End Region


#Region "Public methods"

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates new <b>IDbCommand</b> object using the SQL statement against default <i>BaseDbService</i> connection.
    ''' </summary>
    ''' <param name="SqlText">
    ''' the SQL statement that should be used by <b>IDbCommand</b> as it's <b>CommandText</b>
    ''' </param>
    ''' <param name="Transaction">
    ''' The <b>IDbTransaction</b> object that should be applied to the <b>IDbCommand</b>.
    ''' </param>
    ''' <returns>
    ''' Returns newly created <b>IDbCommand</b> object
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function CreateCommand(ByVal SqlText As String, Optional ByVal Transaction As IDbTransaction = Nothing) As IDbCommand
        Return CreateCommand(SqlText, Connection, Transaction)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Creates new <b>IDbCommand</b> object that should execute stored procedure against default <i>BaseDbService</i> connection.
    ''' </summary>
    ''' <param name="spName">
    ''' the name of the stored procedure
    ''' </param>
    ''' <param name="Transaction">
    ''' The <b>IDbTransaction</b> object that should be applied to the <b>IDbCommand</b>.
    ''' </param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function CreateSPCommand(ByVal spName As String, Optional ByVal Transaction As IDbTransaction = Nothing) As IDbCommand
        Return CreateSPCommand(spName, Connection, Transaction)
    End Function
    Public Function CreateSPCommandWithParams(ByVal SqlText As String, Optional ByVal Transaction As IDbTransaction = Nothing, Optional ByVal paramValues As Dictionary(Of String, Object) = Nothing) As IDbCommand
        Dim cmd As IDbCommand = CreateSPCommand(SqlText, Connection, Transaction)
        StoredProcParamsCache.CreateParameters(cmd, paramValues)
        Return cmd
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Fills the specific table in the <b>DataSet</b> with lookup data translated to the current application 
    ''' language defined by ClobalSettings.CurrentLanguage"property.
    ''' </summary>
    ''' <param name="ds">
    ''' <b>DataSet</b> that contains the lookup table
    ''' </param>
    ''' <param name="spName">
    ''' the name of stored procedure that returns lookup data
    ''' </param>
    ''' <param name="TableName">
    ''' the name of <b>DataTable</b> in the <b>DataSet</b> that will be filled with data.
    ''' </param>
    ''' <param name="Connection">
    ''' connection object that should be used for this operation
    ''' </param>
    ''' <param name="AdditionalParams">
    ''' Optional additional parameters that should be passed to the lookup stored procedure.
    ''' </param>
    ''' <returns>
    ''' Returns <b>True</b> if the lookup table was filled successfully, <b>False</b> if any error occurred.
    ''' </returns>
    ''' <remarks>
    ''' This function assumes that data is retrieved using the stored procedure that can return translated lookup data.
    ''' This stored procedure should follow the next rules:
    ''' <list type="bullet">
    ''' <item>
    ''' <description>
    ''' By default the name of the lookup stored procedure should have the name <i>spTableName_SelectLookup</i>, where TableName is the name 
    ''' of main lookup data table. It is recommended to use this naming convention to avoid compatibility with other parts of framework.
    ''' </description>
    ''' </item>
    ''' <item>
    ''' <description>
    ''' If the lookup data should be translated (IsMultiLanguage=<b>True</b>), this stored procedure must have the <b>nvarchar</b> parameter <i>@LangID</i>.
    ''' </description>
    ''' </item>
    ''' <item>
    ''' <description>
    ''' The stored procedure must return row containing the primary key field and this field must be the first column in the returned result set.
    ''' </description>
    ''' </item>
    ''' <item>
    ''' <description>
    ''' If procedure has some other additional parameters, 
    ''' <i>AdditionalParams</i> <b>Hashtable</b> should contain the key-value pairs for these parameters.
    ''' If the additional stored procedure parameter have no defined value (<b>DBNull.Value</b> should passed to the stored procedure), 
    ''' this value should be described in the <b>Hashtable</b> using the syntax 
    ''' <i>Type:SqlDbType</i>, where SqlDbType is the <b>System.Data.SqlDbType</b> name that corresponds desired parameter type.
    ''' For example to pass NULL parameter of <b>INT</b> type, you should define <i>AdditionalParams</i> item in the next way:
    ''' <code>
    ''' AdditionalParams("ParamName") = "Type:Int"
    ''' </code>
    ''' Additional parameters are usually used to filter lookup data by some criteria. 
    ''' These criteria can be needed outside of this function call. To provide this functionality all additional 
    ''' parameters are stored in the <b>ExtendedProperties</b> property of filled <b>DataTable</b>
    ''' as key-value pair. If NULL additional parameter was passed to the stored procedure, 
    ''' it is stored in <b>ExtendedProperties</b> as empty string.
    ''' You can always retrieve the current value of additional parameter related with the lookup table
    ''' in the next way:
    ''' <code>
    ''' Dim val As String = table.ExtendedProperties("keyName")
    ''' </code>
    ''' </description>
    ''' </item>
    ''' </list>
    ''' Any other requirements for the lookup stored procedure used by this function are not defined.
    ''' </remarks>
    ''' <example>
    ''' The next example shows how to fill the lookup list of organizations and get access to the <b>DataTable</b> containing this list:
    ''' <code>
    ''' BaseDbService.FillLookup(ds, "spOrganization_SelectLookup", "Organization")
    ''' Dim OrganizationsTable As DataTable = ds.Tables("Organization")
    ''' </code>
    ''' Let's consider another situation. For example we have the stored procedure spDivision_SelectLookup that have additional
    ''' parameter OrganizationID. If OrganizationID = NULL, it returns the list of all divisions in the system, in other case it
    ''' returns only divisions related with organization defined by OrganizationID value.  
    ''' To get the list of divisions for the organization having primary key OrganizationID = 10, the next code can be used:
    ''' <code>
    ''' Dim AdditionalParams As New Hashtable()
    ''' AdditionalParams("OrganizationID") = 10
    ''' BaseDbService.FillLookup(ds, "spDivision_SelectLookup", "Division", AdditionalParams)
    ''' Dim OrganizationsTable As DataTable = ds.Tables("Organization")
    ''' Dim CurrOrganizationID As Integer = CInt(OrganizationsTable.ExtendedProperties("OrganizationID"))
    ''' </code>
    ''' The next code demonstrates how to retrieve the entire divisions list:
    ''' <code>
    ''' Dim AdditionalParams As New Hashtable()
    ''' AdditionalParams("OrganizationID") = "Type:Int"
    ''' BaseDbService.FillLookup(ds, "spDivision_SelectLookup", "Division", AdditionalParams)
    ''' Dim OrganizationsTable As DataTable = ds.Tables("Organization")
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FillLookupTable(ByVal ds As DataSet, ByVal spName As String, ByVal TableName As String, ByVal Connection As IDbConnection, Optional ByVal AdditionalParams As Hashtable = Nothing) As Boolean
        DebugTimer.Start(String.Format("{0} lookup filling", TableName))
        Dim cmd As IDbCommand = CreateSPCommand(spName, Connection)
        StoredProcParamsCache.CreateParameters(cmd)
        If cmd.Parameters.Contains("@languageid") Then
            SetParam(cmd, "@languageid", bv.model.Model.Core.ModelUserContext.CurrentLanguage)
        End If
        If cmd.Parameters.Contains("@LangID") Then
            SetParam(cmd, "@LangID", bv.model.Model.Core.ModelUserContext.CurrentLanguage)
        End If
        If Not AdditionalParams Is Nothing Then
            For Each key As String In AdditionalParams.Keys
                Dim val As Object = AdditionalParams(key)
                If Not val Is Nothing AndAlso Not val Is DBNull.Value Then
                    SetParam(cmd, key, val)
                End If
            Next
        End If
        Dim Adapter As DbDataAdapter = CreateAdapter(cmd)
        If ds.Tables.Contains(TableName) Then
            ds.Tables(TableName).BeginLoadData()
            ds.Tables(TableName).Clear()
        End If
        Adapter.Fill(ds, TableName)
        If (ds.Tables(TableName).PrimaryKey Is Nothing) OrElse (ds.Tables(TableName).PrimaryKey.Length = 0) Then
            For i As Integer = 0 To ds.Tables(TableName).Columns.Count - 1
                If ds.Tables(TableName).Columns(i).AllowDBNull = False Then
                    ds.Tables(TableName).PrimaryKey = New DataColumn() {ds.Tables(TableName).Columns(i)}
                    Exit For
                End If
            Next
        End If

        ds.Tables(TableName).EndLoadData()
        If Not AdditionalParams Is Nothing Then
            For Each key As String In AdditionalParams.Keys
                Dim val As Object = AdditionalParams(key)
                If Not val Is Nothing AndAlso Not val Is DBNull.Value Then
                    ds.Tables(TableName).ExtendedProperties(key) = val.ToString
                Else
                    ds.Tables(TableName).ExtendedProperties(key) = ""
                End If
            Next
        End If
        DebugTimer.Stop()
        Return True
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Assigns the specific mapping name to the <b>DataTable</b> object and defines its primary key if needed.
    ''' </summary>
    ''' <param name="dt">
    '''  <b>DataTable</b> object that should be corrected
    ''' </param>
    ''' <param name="TableName">
    ''' the table name that should be assigned to the <b>DataTable.TableName</b> property
    ''' </param>
    ''' <param name="PKFieldName">
    ''' the name of the primary key field for the table
    ''' </param>
    ''' <remarks>
    ''' Some times when complex joins are used in the SQL statement that fills <b>DataTable</b> with data,
    ''' the <b>DataTable.PrimaryKey</b> property left undefined and this can prevent further table data using.
    ''' Call this method immediately after filling <b>DataTable</b> with data to go around this situation.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Sub CorrectTable(ByVal dt As DataTable, Optional ByVal TableName As String = Nothing, Optional ByVal PKFieldName As String = Nothing)
        If Not TableName Is Nothing Then
            dt.TableName = TableName
        End If
        If Not PKFieldName Is Nothing AndAlso dt.Columns.Contains(PKFieldName) Then
            dt.PrimaryKey = New DataColumn() {dt.Columns(PKFieldName)}
        ElseIf dt.PrimaryKey.Length = 0 Then
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
        End If
    End Sub

    Public Shared Sub CorrectTableEx(ByVal dt As DataTable, Optional ByVal TableName As String = Nothing, Optional ByVal PKFieldNames() As String = Nothing)
        If Not TableName Is Nothing Then
            dt.TableName = TableName
        End If
        If dt.PrimaryKey.Length = 0 Then
            If Not PKFieldNames Is Nothing AndAlso Not PKFieldNames.Length > 0 Then
                Dim cols As New List(Of DataColumn)
                For Each fieldName As String In PKFieldNames
                    cols.Add(dt.Columns(fieldName))
                Next
                dt.PrimaryKey = cols.ToArray
            Else
                dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
            End If
        End If
    End Sub


#End Region

#Region "Overridable methods"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns <b>DataSet</b> with entire list of records related with <i>BaseDbService</i> object.
    ''' </summary>
    ''' <returns>
    ''' Returns <b>DataSet</b> with entire list of records related with <i>BaseDbService</i> object.
    ''' </returns>
    ''' <remarks>
    ''' Use this method if you want to retrieve all records related with current <i>BaseDbService</i> object.
    ''' If you want to retrieve the specific page of records use <i>GetPagedList</i> method. To retrieve specific record
    ''' use <i>GetDetail</i> method
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function GetList() As DataSet
        Dim cmd As IDbCommand = Nothing
        If Utils.IsEmpty(SearchProcedureName) Then
            Dim SqlText As String = GetSelectListSql()
            Dim parser As New SqlParser
            parser.SQL = SqlText
            If Utils.Str(ListFromCondition) <> "" Then
                parser.From += String.Format(" {0}", ListFromCondition)
            End If
            If Utils.Str(ListFilterCondition) <> "" Then
                If Not parser.Where Is Nothing AndAlso parser.Where.Trim <> "" Then
                    parser.Where = String.Format("{0} and ({1})", parser.Where, ListFilterCondition)
                Else
                    parser.Where = String.Format("({0})", ListFilterCondition)
                End If
            End If
            SqlText = parser.SQL
            cmd = CreateCommand(SqlText)
        Else
            cmd = CreateSPCommand(SearchProcedureName)
            StoredProcParamsCache.CreateParameters(cmd, SearchParameters)
        End If
        Dim da As DbDataAdapter = CreateAdapter(cmd)
        Dim ds As New DataSet
        da.Fill(ds, ObjectName)
        Return ds
    End Function

    Public Overridable Function GetList(ByVal FilterCondition As String, ByVal FromCondition As String, ByVal SortCondition As String, ByVal OnlyIdentity As Boolean) As DataSet
        Dim cmd As IDbCommand = Nothing
        If Utils.IsEmpty(SearchProcedureName) Then
            Dim SqlText As String = GetSelectListSql(OnlyIdentity)
            Dim parser As New SqlParser
            parser.SQL = SqlText
            If Utils.Str(ListFromCondition) <> "" Then
                parser.From += String.Format(" {0}", ListFromCondition)
            End If
            If Utils.Str(ListFilterCondition) <> "" Then
                If Not parser.Where Is Nothing AndAlso parser.Where.Trim <> "" Then
                    parser.Where = String.Format("{0} and ({1})", parser.Where, ListFilterCondition)
                Else
                    parser.Where = String.Format("({0})", ListFilterCondition)
                End If
            End If
            If Not FilterCondition Is Nothing AndAlso FilterCondition <> "" AndAlso FilterCondition <> ListFilterCondition Then
                If Not parser.Where Is Nothing AndAlso parser.Where.Trim <> "" Then
                    parser.Where = String.Format("{0} and ({1})", parser.Where, FilterCondition)
                Else
                    parser.Where = String.Format("({0})", FilterCondition)
                End If
            End If
            If Not FromCondition Is Nothing And FromCondition <> "" AndAlso FromCondition <> ListFromCondition Then
                parser.From += String.Format(" {0}", FromCondition)
            End If
            If Not SortCondition Is Nothing Then
                parser.Order = SortCondition
            End If
            SqlText = parser.SQL
            cmd = CreateCommand(SqlText)
        Else
            cmd = CreateSPCommand(SearchProcedureName)
            StoredProcParamsCache.CreateParameters(cmd, SearchParameters)
        End If
        Dim da As DbDataAdapter = CreateAdapter(cmd)
        Dim ds As New DataSet
        da.Fill(ds, ObjectName)
        Return ds
    End Function

    Public Overridable Function GetList(ByVal FilterCondition As String, ByVal FromCondition As String, ByVal SortCondition As String) As DataSet
        Return GetList(FilterCondition, FromCondition, SortCondition, False)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns the SQL statement that should be used to retrieve the list 
    ''' of records for object related with current instance of <i>BaseDbService</i>
    ''' </summary>
    ''' <returns>
    ''' Returns the SQL statement that should be used to retrieve the list 
    ''' of records for object related with current instance of <i>BaseDbService</i>
    ''' </returns>
    ''' <remarks>
    ''' By default it is assumed that the records list id retrieved using user defined function 
    ''' with name fn_ObjectName_SelectList that accepts current system language as parameter through 
    ''' the statement 'Select * from fn_ObjectName_SelectList(@LangID)'. 
    ''' Here ObjectName means the <i>ObjectName</i> property of <i>BaseDbService</i> instance. 
    ''' If you want to use the SQL statement of other type, override this function in the descendant class.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	13.04.2006	Created
    ''' </history>
    '''     ''' -----------------------------------------------------------------------------
    Public Overridable Function GetSelectListSql() As String
        Return String.Format("Select {0}fn_{1}_SelectList.* from {0}fn_{1}_SelectList('{2}')", CommandPrefix, ObjectName, bv.model.Model.Core.ModelUserContext.CurrentLanguage)
    End Function

    Public Overridable Function GetSelectListSql(ByVal OnlyIdentity As Boolean) As String
        If (OnlyIdentity AndAlso IdentityFieldName <> Nothing) Then
            Return String.Format("Select TOP 10000 {0}fn_{1}_SelectList.{3} from {0}fn_{1}_SelectList('{2}')", CommandPrefix, ObjectName, bv.model.Model.Core.ModelUserContext.CurrentLanguage, IdentityFieldName)
        End If
        Return String.Format("Select TOP 10000 {0}fn_{1}_SelectList.* from {0}fn_{1}_SelectList('{2}')", CommandPrefix, ObjectName, bv.model.Model.Core.ModelUserContext.CurrentLanguage)
    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns the requested page of sorted and filtered records related with the main service object.
    ''' </summary>
    ''' <param name="PageSize">
    ''' the number records in the page of data
    ''' </param>
    ''' <param name="PageNumber">
    ''' the page of records that should be returned. The absolute number of the first record in the returned dataset will be equal <i>PageNumber*Pagesize+1</i>
    ''' </param>
    ''' <param name="FilterCondition">
    ''' the additional filter that will be added to the WHERE clause of the default SQL statement used to retrieve data.
    ''' </param>
    ''' <param name="FromCondition">
    ''' the additional condition that will be added to the FROM clause of the default SQL statement used to retrieve data.
    ''' it can be used to create complex data filtering using joined tables.
    ''' </param>
    ''' <param name="SortCondition">
    ''' the sort expression that will be used in the ORDER BY clause of the default SQL statement used to retrieve data.
    ''' </param>
    ''' <param name="RecordCount">
    ''' returns approximate record count returned by the method. This number defines how many pages 
    ''' should be displayed by the grid pager. Depending on implementation it can be or exact 
    ''' record count matching current SQL statement or minimal number needed to define how many pages 
    ''' should display grid pager. <br/>
    ''' <b>Note:</b> current implementation returns exact records count matching current SQL statement.
    ''' </param>
    ''' <returns>
    ''' <b>DataSet</b> with records that match requested parameters
    ''' </returns>
    ''' <remarks>
    ''' <i>GetPagedList</i> method is used by descendant of <i>BasePageListForm</i> class to retrieve
    ''' data for <i>BasePagedDataGrid</i> control that displays these data.
    ''' Default <i>GetPagedList</i> method implementation can be overridden in the descendant classes,
    ''' but usually this is not needed.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function GetPagedList(ByVal PageSize As Integer, ByVal PageNumber As Integer, ByVal FilterCondition As String, ByVal FromCondition As String, _
                ByVal SortCondition As String, ByRef RecordCount As Integer) As DataSet
        Dim cmd As IDbCommand = CreateCommand(GetSelectListSql)
        Dim da As DbDataAdapter = CreateAdapter(cmd)
        Dim ds As New DataSet

        Dim p As Pager = New Pager(da)
        p.SkipValidation = SkipSqlValidation
        p.PageSize = PageSize

        If Utils.Str(ListFromCondition) <> "" Then
            p.From += String.Format(" {0}", ListFromCondition)
        End If
        If Utils.Str(ListFilterCondition) <> "" Then
            If Not p.Where Is Nothing AndAlso p.Where.Trim <> "" Then
                p.Where = String.Format("{0} and ({1})", p.Where, ListFilterCondition)
            Else
                p.Where = String.Format("({0})", ListFilterCondition)
            End If
        End If

        If Not FilterCondition Is Nothing AndAlso FilterCondition <> "" AndAlso FilterCondition <> ListFilterCondition Then
            If Not p.Where Is Nothing AndAlso p.Where.Trim <> "" Then
                p.Where = String.Format("{0} and ({1})", p.Where, FilterCondition)
            Else
                p.Where = String.Format("({0})", FilterCondition)
            End If
        End If
        If Not FromCondition Is Nothing And FromCondition <> "" AndAlso FromCondition <> ListFromCondition Then
            p.From = String.Format("{0} {1}", p.From, FromCondition)
        End If
        If Not SortCondition Is Nothing Then
            p.Order = SortCondition
        End If
        'ListFromCondition = FromCondition
        'ListFilterCondition = FilterCondition
        p.GetPage(ds, ObjectName, PageNumber)
        RecordCount = p.ApproximateRecordCount
        Return ds

    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Fills <b>DataSet</b> related with single instance of specific object. 
    ''' </summary>
    ''' <param name="ID">
    ''' the primary key of the object. 
    ''' </param>
    ''' <returns>
    ''' Returns the <b>DataSet</b> filled with data related with single instance of specific object.
    ''' </returns>
    ''' <remarks>
    ''' <i>GetDetail</i> method is used by classes descendent from <i>BaseDetailForm</i> or <i>BaseWizardForm</i>
    ''' to retrieve detail object information.
    ''' Override <i>GetDetail</i> method if descendent class should be used for processing single object data.
    ''' <i>ID</i> parameter can be represented as instance of standard simple object 
    ''' (e.g. <b>Integer</b>, <b>Guid</b> or <b>String</b>) as arbitrary custom class that describes
    ''' object primary key. The EIDSS/PASC databases don't use compound primary keys, so <i>ID</i> is
    ''' represented by the standard objects usually. If <b>Nothing</b> is passed as  <i>ID</i> value
    ''' the <b>DataSet</b> filled with new object data should be returned.<br/>
    ''' The implementation of this method in descendant classes is sufficiently arbitrary, you just should know 
    ''' the implementation details and take them into account when use it.
    ''' The default recommendation are: <br/>
    ''' 1. The data related with main object should be contained in the first record of the first dataset table.<br/>
    ''' 2. The first dataset table name should be the same as <i>ObjectName</i> property.<br/>
    ''' 3. The table related with the main object should contain at least one record. If new object is created, the 
    ''' first record should contain the row with default object values.
    ''' 4. It is recommended to initialize <i>m_ID</i> member inside this method to make object identificator
    ''' accessible in other class methods
    ''' </remarks>
    ''' <example>
    ''' The simplest implementation of <i>LoadDetail</i> method demonstrates how to fill dataset with 
    ''' object data and initialize protected <i>m_Error</i> and <i>m_ID</i> members. It is assumed that
    ''' the class descendant from <i>BaseDbService</i> works with the object named <c>MyObjectName</c> 
    ''' and uses <c>spMyObjectName_SelectDetail</c> stored procedure to retrieve object data
    ''' <code>
    '''Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
    '''    Dim ds As New DataSet
    '''    Try
    '''        'Create command related with the stored procedure spMyObjectName_SelectDetail
    '''        Dim cmd As IDbCommand = CreateSPCommand("spMyObjectName_SelectDetail")
    '''        AddParam(cmd, "@ID", ID)
    '''        'Create the data adapter based on the this command
    '''        Dim adapter As DbDataAdapter = CreateAdapter(cmd)
    '''        'and fill dataset with object data
    '''        adapter.Fill(ds, "MyObjectName")
    '''        'Process the new object creation
    '''        'It is assumed that if ID is nothing we should return 
    '''        'the dataset containing empty row related with the main obiect
    '''        If ID Is Nothing Then
    '''            Dim r As DataRow = ds.Tables("MyObjectName").NewRow()
    '''            ID = Guid.NewGuid
    '''            r("ID") = ID
    '''            ds.EnforceConstraints = False
    '''            ds.Tables("MyObjectName").Rows.Add(r)
    '''        End If
    '''        'initialize  m_ID to link the class instance with specific primary key
    '''        m_ID = ID
    '''        Return ds
    '''    Catch ex As Exception
    '''        'Set the last error pointer to the current error
    '''        m_Error = New ErrorMessage(StandardError.FillDatasetError, ex)
    '''        Return Nothing
    '''    End Try
    '''    Return Nothing
    '''End Function
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function GetDetail(ByVal ID As Object) As DataSet
        m_ID = ID
        If ID Is Nothing Then
            m_IsNewObject = True
        End If
        Return Nothing
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Called inside <i>Post</i> method before <i>PostDetail</i> method call. Override this method 
    ''' if you need to perform some database or dataset operation before main posting procedure.
    ''' <i>BeforePost</i> method is called outside of transaction, so all database operations 
    ''' performed inside this method will not roll backed if <i>PostDetail</i> method will be failed
    ''' and roll backed.
    ''' </summary>
    ''' <param name="ds">
    ''' <b>DataSet</b> object containing data that should be posted to database
    ''' </param>
    ''' <param name="PostType">
    ''' defines the type of the data posting. Usually <i>PostType</i> enumeration is used for this argument, 
    ''' but it is possible to use any other number, you should just now what any specific value is mean 
    ''' and process it in correct way
    ''' </param>
    ''' <returns>
    ''' Returns <b>True</b> if method was completed successfully or <b>False</b> in other case.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Public Overridable Function BeforePost(ByVal ds As DataSet, ByVal PostType As Integer) As Boolean
        'For Each childservice As ServiceParam In m_LinkedServices
        '    If (RaiseChildPostEvents(childservice.service, PostType, 0) = False) Then
        '        Return False
        '    End If
        'Next
        Dim e As PostEventArgs = New PostEventArgs(PostType, False)
        RaiseEvent OnBeforePost(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function
    Public Overridable Function TransactionStarted(ByVal PostType As Integer, ByVal transaction As IDbTransaction) As Boolean
        Dim e As PostEventArgs = New PostEventArgs(PostType, False, transaction)
        RaiseEvent OnTransactionStarted(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function
    Public Overridable Function TransactionFinished(ByVal PostType As Integer) As Boolean
        Dim e As PostEventArgs = New PostEventArgs(PostType, False, Nothing)
        RaiseEvent OnTransactionFinished(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function

    Public Overridable Function TransactionRollBack(ByVal PostType As Integer) As Boolean
        Dim e As PostEventArgs = New PostEventArgs(PostType, False, Nothing)
        RaiseEvent OnTransactionRollBack(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function
    Public Overridable Function BeforeTransactionCommit(ByVal PostType As Integer, ByVal transaction As IDbTransaction) As Boolean
        Dim e As PostEventArgs = New PostEventArgs(PostType, False, transaction)
        RaiseEvent OnBeforeTransactionCommit(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Called inside <i>Post</i> method after <i>PostDetail</i> method call. Override this method 
    ''' if you need to perform some database or dataset operation after main posting procedure.
    ''' <i>AfterPost</i> method is called outside of main transaction, so all database operations 
    ''' performed inside <i>PostDetail</i> method will not roll backed if this method will be failed
    ''' and roll backed. 
    ''' </summary>
    ''' <param name="ds">
    ''' <b>DataSet</b> object containing data that should be posted to database
    ''' </param>
    ''' <param name="PostType">
    ''' defines the type of the data posting. Usually <i>PostType</i> enumeration is used for this argument, 
    ''' but it is possible to use any other number, you should just now what any specific value is mean 
    ''' and process it in correct way
    ''' </param>
    ''' <returns>
    ''' Returns <b>True</b> if method was completed successfully or <b>False</b> in other case.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function AfterPost(ByVal ds As DataSet, ByVal PostType As Integer) As Boolean
        Dim e As PostEventArgs = New PostEventArgs(PostType, False)
        RaiseEvent OnAfterPost(Me, e)
        If e.Cancel = True Then Return False
        Return True
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The main method that should be used to save <b>DataSet</b> data. Each descendant class that should
    ''' save the data must override this method. This method is called inside <i>Post</i> method and performed
    ''' within one transaction. If the method is failed, all data posted to the database before error will be 
    ''' roll backed.
    ''' </summary>
    ''' <param name="ds">
    ''' <b>DataSet</b> object containing data that should be posted to database
    ''' </param>
    ''' <param name="PostType">
    ''' defines the type of the data posting. Usually <i>PostType</i> enumeration is used for this argument, 
    ''' but it is possible to use any other number, you should just now what any specific value is mean 
    ''' and process it in correct way
    ''' </param>
    ''' <param name="transaction">
    ''' <b>IDbTransaction</b> object that is used for data posting
    ''' </param>
    ''' <returns>
    ''' Returns <b>True</b> if method was completed successfully or <b>False</b> in other case.
    ''' </returns>
    ''' <remarks>
    ''' If the <i>transaction</i> parameter is passed to the method, all data adapters/commands used for data
    ''' posting should use the <i>transaction</i>. Call <i>ApplyTtransaction</i> method for each
    ''' data adapter used inside <i>PostDetail</i> for this purpose.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function PostDetail(ByVal ds As DataSet, ByVal PostType As Integer, Optional ByVal transaction As IDbTransaction = Nothing) As Boolean
        Return IgnoreChanges
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Deletes the object related with <i>BaseDbService</i> instance. Default implementation assumes that object 
    ''' should be deleted by the stored procedure named <i>sp<b>ObjectName</b>_Delete</i>, where <i><b>ObjectName</b></i>
    ''' is the value of <i>BaseDbService</i> <i>ObjectName</i> property.<br/>
    ''' Override this method if you need other object deleting implementation.
    ''' </summary>
    ''' <param name="ID">
    ''' unique identifier of object to delete
    ''' </param>
    ''' <returns>
    ''' <b>Nothing</b> if object was deleted successfully or <i>ErrorMessage</i> if error occurs.
    ''' </returns>
    ''' <remarks>
    ''' <i>Delete</i> method is called by <i>BaseListForm</i> descendant classes for current grid row 
    ''' when <b>Delete</b> button is pressed.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function Delete(ByVal ID As Object) As Boolean
        If Utils.IsEmpty(ID) Then Return True
        Dim transaction As IDbTransaction = Nothing
        SyncLock Connection
            Try
                If (Connection.State And ConnectionState.Open) = 0 Then
                    Connection.Open()
                End If
                transaction = Connection.BeginTransaction()
                TransactionStarted(0, transaction)
                If Delete(ID, transaction) Then
                    AuditManager.ClearAuditContext(transaction, Connection)
                    transaction.Commit()
                    transaction = Nothing
                    m_ID = Nothing
                    TransactionFinished(0)
                    Return True
                End If
            Catch ex As Exception
                Throw
            Finally
                If Not transaction Is Nothing Then
                    AuditManager.ClearAuditContext(transaction, Connection)
                    transaction.Rollback()
                    transaction = Nothing
                End If
                If Connection.State = ConnectionState.Open Then Connection.Close()
            End Try
            Return False
        End SyncLock
    End Function

    Public Overridable Function Delete(ByVal ID As Object, ByVal transaction As IDbTransaction) As Boolean
        Dim cmd As IDbCommand = CreateSPCommand(String.Format("{0}sp{1}_Delete", CommandPrefix, ObjectName), Connection, transaction)
        StoredProcParamsCache.CreateParameters(cmd)
        CType(cmd.Parameters(0), SqlParameter).Value = ID
        m_Error = ExecCommand(cmd, Connection, transaction)
        Return m_Error Is Nothing
    End Function

    Public Overridable Function CanDelete(ByVal ID As Object, Optional ByVal aObjectName As String = Nothing, Optional ByVal transaction As IDbTransaction = Nothing) As Boolean
        If Utils.Str(aObjectName) = "" Then
            aObjectName = ObjectName
        End If
        Dim cmd As IDbCommand = CreateSPCommand("spCanDelete", Connection)
        AddParam(cmd, "@ObjectName", aObjectName)
        AddParam(cmd, "@ID", ID)
        AddParam(cmd, "@Result", False, ParameterDirection.InputOutput)
        m_Error = ExecCommand(cmd, Connection, transaction)
        If m_Error Is Nothing Then
            Return CType(GetParamValue(cmd, "@Result"), Boolean)
        End If
        Return False
    End Function

    Public Function LoadDetailData(objectId As Object) As DataSet
        m_IsNewObject = False
        Return GetDetail(objectId)
    End Function
    'we will perform one additional posting attempt if transaction lock occur
    Private m_IsTransactionLockMode As Boolean = False

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Saves data from <b>DataSet</b> to database.
    ''' </summary>
    ''' <param name="ds">
    ''' <b>DataSet</b> object containing data that should be posted to database
    ''' </param>
    ''' <param name="PostType">
    ''' defines the type of the data posting. Usually <i>PostType</i> enumeration is used for this argument, 
    ''' but it is possible to use any other number, you should just now what any specific value is mean 
    ''' and process it in correct way
    ''' </param>
    ''' <returns>
    ''' Returns <b>True</b> if method was completed successfully or <b>False</b> in other case.
    ''' </returns>
    ''' <remarks>
    ''' This method is called automatically by <b>BaseForm</b> descendant classes when <b>OK</b> button
    ''' is pressed. It should be also called explicitly if any jump to other child form that should 
    ''' use parent form data is performed.
    ''' </remarks>
    ''' <example>
    ''' For example if we edit Order form containing the set of OrderDetail records and want to create
    ''' new OrderDetail object for Order the next code can be used:
    ''' <code>
    ''' Private Sub btnNewOrderDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewOrderDetail.Click
    '''    'Saves current form data as intermediate data 
    '''    If Post(PostType.IntermediatePosting) = False Then Exit Sub
    '''    'Show the OrderDetailDetail as child of the current form
    '''    If BaseForm.ShowModal(New OrderDetailDetail, Nothing, GetKey()) = True Then
    '''        'Reload data to add new OrderDetail object records to the form dataset 
    '''        LoadData(GetKey())
    '''    End If
    ''' End Sub
    ''' </code>
    ''' </example>    ''' 
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    ''' 

    Public Function Post(ByVal ds As DataSet, Optional ByVal PostType As Integer = 0) As Boolean
#If PROTOTYPE_MODE = True Then
        Return True
#End If
        Dim transaction As IDbTransaction = Nothing
        Dim IgnoreMainPost As Boolean
        Dim dsCopy As DataSet

        If BeforePost(ds, PostType) = False Then
            Return False
        End If
        SyncLock Connection
            Try
                If (Connection.State <> ConnectionState.Open) Then
                    Connection.Open()
                End If
                transaction = Connection.BeginTransaction()
                TransactionStarted(PostType, transaction)
                DatasetCopies.Clear()
                IgnoreMainPost = False

                For Each service As ServiceParam In m_LinkedServices
                    If service.serviceType = RelatedPostOrder.PostFirst OrElse _
                           service.serviceType = RelatedPostOrder.PostInstead Then
                        If PostRelatedDetail(service.service, PostType, transaction) = False Then
                            Return False
                        ElseIf service.serviceType = RelatedPostOrder.PostInstead Then
                            IgnoreMainPost = True
                        End If
                    End If
                Next
                If (UseDatasetCopyInPost = True) Then
                    dsCopy = ds.Copy
                Else
                    dsCopy = ds
                End If
                If IgnoreMainPost = True OrElse PostDetail(dsCopy, PostType, transaction) = True Then
                    For Each service As ServiceParam In m_LinkedServices
                        If service.serviceType = RelatedPostOrder.PostLast Then
                            If PostRelatedDetail(service.service, PostType, transaction) = False Then
                                Return False
                            End If
                        End If
                    Next
                    AuditManager.ClearAuditContext(transaction, Connection)
                    BeforeTransactionCommit(PostType, transaction)
                    transaction.Commit()
                    transaction = Nothing
                    TransactionFinished(PostType)
                    If (UseDatasetCopyInPost = True) Then
                        Merge(ds, dsCopy) 'merge changes that posibly was done during posting
                    End If
                    RaiseBeforeAcceptChangesEvent(ds)
                    AcceptChanges(ds)
                    AcceptChangesInRelatedSerives(Me)
                    If AfterPost(ds, PostType) = False Then
                        Return False
                    End If
                    m_IsNewObject = False
                    Return True
                End If
                Return False
            Catch e As Exception
                'if transaction lock error occurs try to repeat post ofter some timeout
                If TypeOf (e) Is SqlException _
                        AndAlso CType(e, SqlException).ErrorCode = 1205 _
                        AndAlso m_IsTransactionLockMode = False Then
                    Thread.Sleep(2000)
                    m_IsTransactionLockMode = True 'this is the only condition that allows go to code after finally block
                Else
                    m_Error = New ErrorMessage(StandardError.PostError, e)
                    Return False
                End If

            Finally
                If Not transaction Is Nothing Then
                    If Not transaction.Connection Is Nothing Then
                        AuditManager.ClearAuditContext(transaction, Connection)
                        transaction.Rollback()
                        TransactionRollBack(PostType)
                    End If
                    transaction = Nothing
                End If
                DatasetCopies.Clear()
                If Connection.State = ConnectionState.Open Then Connection.Close()
            End Try
        End SyncLock
        If m_IsTransactionLockMode Then
            Dim result As Boolean = Post(ds, PostType)
            m_IsTransactionLockMode = False
            Return result
        End If
    End Function

    Private DatasetCopies As New Hashtable
    Protected Function PostRelatedDetail(ByVal service As BaseDbService, Optional ByVal PostType As Integer = 0, Optional ByVal transaction As IDbTransaction = Nothing) As Boolean

        For Each childservice As ServiceParam In service.m_LinkedServices
            If childservice.serviceType = RelatedPostOrder.PostFirst OrElse _
                   childservice.serviceType = RelatedPostOrder.PostInstead Then
                If PostRelatedDetail(childservice.service, PostType, transaction) = False Then
                    Return False
                ElseIf childservice.serviceType = RelatedPostOrder.PostInstead Then
                    Return True
                End If
            End If
        Next
        Dim sParam As ServiceParam = service.ParentService.FindLinkedServiceParams(service)
        If Not sParam Is Nothing Then

            Dim ds1 As DataSet = Nothing
            If Not sParam.dataSet Is Nothing Then
                Try
                    If service.BeforePost(sParam.dataSet, sParam.serviceType) = False Then
                        m_Error = service.LastError
                        If Not m_Error.Exception Is Nothing AndAlso TypeOf m_Error.Exception Is System.Data.ConstraintException Then
                            DataDiag.PrintDataSetConstraintDiagnostics(sParam.dataSet)
                        End If
                        Return False
                    End If

                    Dim enableConstraints As Boolean = sParam.dataSet.EnforceConstraints
                    sParam.dataSet.EnforceConstraints = False
                    If UseDatasetCopyInPost Then
                        ds1 = sParam.dataSet.Copy()
                        DatasetCopies(sParam.dataSet) = ds1
                    Else
                        ds1 = sParam.dataSet
                    End If
                    sParam.dataSet.EnforceConstraints = enableConstraints

                Catch ex As Exception
                    DataDiag.PrintDataSetConstraintDiagnostics(ds1)
                    Throw
                End Try
            End If
            If service.PostDetail(ds1, PostType, transaction) = False Then
                m_Error = service.m_Error
                If Not m_Error Is Nothing AndAlso Not m_Error.Exception Is Nothing AndAlso TypeOf m_Error.Exception Is System.Data.ConstraintException Then
                    DataDiag.PrintDataSetConstraintDiagnostics(ds1)
                End If
                Return False
            Else
                service.AfterPost(sParam.dataSet, sParam.serviceType)
            End If
        End If
        For Each childservice As ServiceParam In service.m_LinkedServices
            If childservice.serviceType = RelatedPostOrder.PostLast Then
                If PostRelatedDetail(childservice.service, PostType, transaction) = False Then
                    m_Error = childservice.service.LastError
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Public Event OnAcceptChanges As EventHandler
    Protected Sub RaiseAcceptChangesEvent(ByVal ds As DataSet)
        RaiseEvent OnAcceptChanges(Me, EventArgs.Empty)
    End Sub
    Public Event OnBeforeAcceptChanges As EventHandler
    Private Sub RaiseBeforeAcceptChangesEvent(ByVal ds As DataSet)
        RaiseEvent OnBeforeAcceptChanges(ds, EventArgs.Empty)
    End Sub
    Protected Sub AcceptChangesInRelatedSerives(ByVal service As BaseDbService)
        If Not service.ParentService Is Nothing Then
            Dim sParam As ServiceParam = service.ParentService.FindLinkedServiceParams(service)
            If Not sParam Is Nothing AndAlso Not sParam.dataSet Is Nothing Then
                If Not DatasetCopies.Contains(sParam.dataSet) Then
                    service.RaiseBeforeAcceptChangesEvent(sParam.dataSet)
                End If
                service.AcceptChanges(sParam.dataSet)
                If DatasetCopies.Contains(sParam.dataSet) Then
                    Merge(sParam.dataSet, CType(DatasetCopies(sParam.dataSet), DataSet))
                    service.RaiseBeforeAcceptChangesEvent(sParam.dataSet)
                    service.AcceptChanges(sParam.dataSet)
                End If
            End If
        End If
        For Each childservice As ServiceParam In service.m_LinkedServices
            AcceptChangesInRelatedSerives(childservice.service)
        Next

    End Sub
    Protected Function RaiseChildPostEvents(ByVal service As BaseDbService, ByVal postType As Integer, ByVal eventType As Integer) As Boolean
        Dim sParam As ServiceParam = FindLinkedServiceParams(service)
        If sParam Is Nothing Then Return False
        If eventType = 0 Then 'BeforePost
            If service.BeforePost(sParam.dataSet, sParam.serviceType) = False Then
                Return False
            End If
        Else
            If service.AfterPost(sParam.dataSet, sParam.serviceType) = False Then
                Return False
            End If
        End If
        Return True
    End Function
    Private m_ParentService As BaseDbService
    Public ReadOnly Property ParentService() As BaseDbService
        Get
            Return m_ParentService
        End Get
    End Property

    Protected m_LinkedServices As New ArrayList
    Protected Class ServiceParam
        Public dataSet As DataSet
        Public serviceType As RelatedPostOrder
        Public service As BaseDbService
        Public Sub New(ByVal ds As DataSet, ByVal sType As RelatedPostOrder, ByVal aService As BaseDbService)
            dataSet = ds
            serviceType = sType
            service = aService
        End Sub
    End Class
    Public Function FindLinkedServiceByType(ByVal aServiceType As System.Type, ByRef ds As DataSet) As BaseDbService
        For Each childService As ServiceParam In m_LinkedServices
            If childService.service.GetType().FullName = aServiceType.FullName Then
                ds = childService.dataSet
                Return childService.service
            End If
        Next
        Return Nothing
    End Function
    Public Function FindLinkedServiceByType(ByVal aServiceType As System.Type) As BaseDbService
        For Each childService As ServiceParam In m_LinkedServices
            If childService.service.GetType().FullName = aServiceType.FullName Then
                Return childService.service
            End If
        Next
        Return Nothing
    End Function


    Private Function FindLinkedServiceParams(ByVal service As BaseDbService) As ServiceParam
        For Each sParam As ServiceParam In m_LinkedServices
            If sParam.service Is service Then
                Return sParam
            End If
        Next
        Return Nothing
    End Function

    Public Sub AddLinkedDbService(ByVal service As BaseDbService, ByVal ds As DataSet, Optional ByVal serviceType As RelatedPostOrder = RelatedPostOrder.PostFirst)
        If service Is Nothing Then Return
        If FindLinkedServiceParams(service) Is Nothing Then
            m_LinkedServices.Add(New ServiceParam(ds, serviceType, service))
            If Not service.ParentService Is Nothing Then
                service.ParentService.RemoveLinkedDbService(service)
            End If
            service.m_ParentService = Me
        End If
    End Sub
    Public Sub RemoveLinkedDbService(ByVal service As BaseDbService)
        If service Is Nothing Then Return
        Dim sParam As ServiceParam = FindLinkedServiceParams(service)
        If Not sParam Is Nothing Then
            service.m_ParentService = Nothing
            m_LinkedServices.Remove(sParam)
        End If
    End Sub
#End Region

#Region "Public Properties"
    Private m_ObjectName As String
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets or sets the name of the object with which <i>BaseDbService</i> class works.
    ''' </summary>
    ''' <remarks>
    ''' <i>ObjectName</i> is used as name-forming string in some default <i>BaseDbService</i> methods implementations.
    ''' It is assumed that <i>ObjectName</i> is included to the related forms/stored procedure/user defined function names:
    ''' <list type="table">
    ''' <item>
    ''' <term>fn_<b>ObjectName</b>_SelectList</term>
    ''' <description>the name of user defined function that returns the list set of records related with the object</description>
    ''' </item>
    ''' <item>
    ''' <term>sp<b>ObjectName</b>_SelectDetail</term>
    ''' <description>the name of stored procedure that returns data related with single object</description>
    ''' </item>
    ''' <item>
    ''' <term>sp<b>ObjectName</b>_Delete</term>
    ''' <description>the name of stored procedure that deletes the specific object</description>
    ''' </item>
    ''' <item>
    ''' <term>sp<b>ObjectName</b>_SelectLookup</term>
    ''' <description>the name of stored procedure that returns data the list of lookup data</description>
    ''' </item>
    ''' <item>
    ''' <term><b>ObjectName</b>List</term>
    ''' <description>the name of the form that displays the list of object records</description>
    ''' </item>
    ''' <item>
    ''' <term><b>ObjectName</b>Detail</term>
    ''' <description>the name of the form that displays the single object data</description>
    ''' </item>
    ''' </list>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property ObjectName() As String
        Get
            'If m_ObjectName Is Nothing OrElse m_ObjectName.Trim = "" Then
            '    Throw New Exception("Object name is not defined")
            'End If
            Return m_ObjectName
        End Get
        Set(ByVal Value As String)
            m_ObjectName = Value
        End Set
    End Property


    Private m_IdentityFieldName As String
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets or sets the name of the identity field name.
    ''' </summary>
    ''' <history>
    ''' 	[Vdovin]	07.10.2010	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property IdentityFieldName() As String
        Get
            Return m_IdentityFieldName
        End Get
        Set(ByVal Value As String)
            m_IdentityFieldName = Value
        End Set
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets or sets <b>IDbConnection</b> that is used to perform all database operations within this class
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private m_ConnectionManager As ConnectionManager
    Public ReadOnly Property Connection() As IDbConnection
        Get
            If m_ConnectionManager Is Nothing Then
                CreateConnection()
            End If
            Return m_ConnectionManager.Connection
        End Get
    End Property

    Public ReadOnly Property ConnectionManager() As ConnectionManager
        Get
            If m_ConnectionManager Is Nothing Then
                CreateConnection()
            End If
            Return m_ConnectionManager
        End Get
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns unique identifier of the object related with the class instance
    ''' </summary>
    ''' <returns>
    ''' Returns unique identifier of the object related with the class instance
    ''' </returns>
    ''' <remarks>
    ''' To use this property you should initialize protected <i>m_ID</i> member inside 
    ''' overridden <i>GetDetail</i> method.
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property ID() As Object
        Get
            Return m_ID
        End Get
        Set(value As Object)
            m_ID = value
        End Set
    End Property
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the last error that occurs inside <i>BaseDbService</i> methods.
    ''' </summary>
    ''' <returns>
    ''' <i>ErrorMessage</i> related with the last error that occurs inside <i>BaseDbService</i> methods.
    ''' </returns>
    ''' <remarks>
    ''' Use this method if <i>ErrorMessage</i> is not returned explicitly by the method itself. If you call 
    ''' this method the any next call will return <b>Nothing</b> before next error occurring.
    ''' </remarks>
    ''' <example>
    ''' <code>
    ''' If MyDbService.Post(ds,0) = False Then
    '''     ErrorForm.ShowError(MyDbService.LastError)
    ''' End If
    ''' </code>
    ''' </example>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property LastError() As ErrorMessage
        Get
            Dim e As ErrorMessage = m_Error
            m_Error = Nothing
            Return e
        End Get
    End Property
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Indicates was the <i>BaseDbService</i> instance used to create new object or no.
    ''' </summary>
    ''' <returns>
    ''' <b>True</b> if <i>BaseDbService</i> instance was used to create new object, <b>False</b> in other case
    ''' </returns>
    ''' <remarks>
    ''' To make this property usable you should initialize protected <i>m_IsNewObject</i> member in the overridden 
    ''' <see cref="BaseDbService.GetDetail"/> method
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	17.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property IsNewObject() As Boolean
        Get
            Return m_IsNewObject
        End Get
    End Property

    Private m_CommandPrefix As String = ""
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gets or sets the prefix that will be inserted before names of all stored procedures/user defined functions
    ''' used by <i>BaseDbService</i> class by default. Returns the empty string by default.
    ''' </summary>
    ''' <remarks>
    ''' Use this property to define the common prefix used before all stored procedures/user defined functions.
    ''' If <i>CommandPrefix</i> is defined you can omit this prefix in the stored procedure/user defined function name.
    ''' For example, if all stored procedures use the prefix <c>dbo.</c>, you can set the <i>CommandPrefix</i> to this value
    ''' and omit it in the stored procedures names
    ''' </remarks>
    ''' <history>
    ''' 	[Mike]	18.04.2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property CommandPrefix() As String
        Get
            Return m_CommandPrefix
        End Get
        Set(ByVal Value As String)
            m_CommandPrefix = Value
        End Set
    End Property
    Private m_IgnoreChanges As Boolean = True
    Public Overridable Property IgnoreChanges() As Boolean
        Get
            Return m_IgnoreChanges
        End Get
        Set(ByVal Value As Boolean)
            m_IgnoreChanges = Value
        End Set
    End Property

#End Region
    Public Shared Sub SetReadOnlyColumnValue(ByVal row As DataRow, ByVal value As Object, ByVal FieldName As String)
        row.Table.Columns(FieldName).ReadOnly = False
        row.BeginEdit()
        row(FieldName) = value
        row.EndEdit()
        row.Table.Columns(FieldName).ReadOnly = True
    End Sub

    Public Shared Sub ForceTableChanges(ByVal dt As DataTable, Optional ByVal UndoManually As Boolean = False)
        dt.ExtendedProperties("SkipAcceptChanges") = True
        If UndoManually Then
            dt.ExtendedProperties("UndoManually") = True
        End If
    End Sub
    Public Shared Function SkipAcceptChanges(ByVal dt As DataTable) As Boolean
        Return dt.ExtendedProperties.ContainsKey("SkipAcceptChanges") AndAlso dt.ExtendedProperties("SkipAcceptChanges").ToString = "True"
    End Function

    Public Shared Sub ClearSkipChanges(ByVal dt As DataTable)
        If dt.ExtendedProperties.ContainsKey("SkipAcceptChanges") Then
            dt.ExtendedProperties.Remove("SkipAcceptChanges")
        End If
        If dt.ExtendedProperties.ContainsKey("UndoManually") Then
            dt.ExtendedProperties.Remove("UndoManually")
        End If
    End Sub


    Public Overridable Sub AcceptChanges(ByVal ds As DataSet)
        ' if you change this method, don't forget to change BaseRamDbService.AcceptChanges 
        ' because BaseRamDbService.AcceptChanges doesn't call base AcceptChanges
        ' Note that BaseRamDbService.AcceptChanges shouldn't has line  m_IsNewObject = False
        For i As Integer = ds.Tables.Count - 1 To 0 Step -1
            Dim table As DataTable = ds.Tables(i)
            If SkipAcceptChanges(table) = False Then
                table.AcceptChanges()
            End If
        Next
        RaiseAcceptChangesEvent(ds)
        m_IsNewObject = False
    End Sub

    Private m_FromCondition As String
    Public Property ListFromCondition() As String
        Get
            Return m_FromCondition
        End Get
        Set(ByVal Value As String)
            m_FromCondition = Value
        End Set
    End Property

    Private m_FilterCondition As String
    Public Property ListFilterCondition() As String
        Get
            Return m_FilterCondition
        End Get
        Set(ByVal Value As String)
            m_FilterCondition = Value
        End Set
    End Property

    Private m_DoClearConditions As Boolean = False
    Public Property DoClearListConditions() As Boolean
        Get
            Return m_DoClearConditions
        End Get
        Set(ByVal value As Boolean)
            m_DoClearConditions = value
        End Set
    End Property

    Public Event OnBeforePost As PostHandler
    Public Event OnAfterPost As PostHandler
    Public Event OnTransactionStarted As PostHandler
    Public Event OnBeforeTransactionCommit As PostHandler
    Public Event OnTransactionFinished As PostHandler
    Public Event OnTransactionRollBack As PostHandler

    Protected Shared Sub OnStateChange(ByVal sender As Object, ByVal args As StateChangeEventArgs)
        If args.CurrentState = ConnectionState.Open Then
            Dim command As IDbCommand = CreateSPCommand("spSetContext", CType(sender, IDbConnection))
            BaseDbService.AddParam(command, "@ContextString", ModelUserContext.ClientID)
            ExecCommand(command, CType(sender, IDbConnection))
        End If
    End Sub
    Public Shared Function CloneParam(ByVal param As IDbDataParameter) As IDbDataParameter
        Dim p As New SqlParameter
        p.ParameterName = param.ParameterName
        p.SourceColumn = param.SourceColumn
        p.SqlDbType = CType(param, SqlParameter).SqlDbType
        p.Size = param.Size
        p.Value = param.Value
        p.Direction = param.Direction
        p.DbType = param.DbType
        Return p
    End Function
    Public Shared Function CloneCommand(ByVal cmd As IDbCommand) As IDbCommand
        Dim cmd1 As New SqlCommand
        cmd1.CommandText = cmd.CommandText
        cmd1.CommandTimeout = cmd.CommandTimeout
        cmd1.CommandType = cmd.CommandType
        cmd1.Connection = CType(cmd.Connection, SqlConnection)
        cmd1.Transaction = CType(cmd.Transaction, SqlTransaction)
        For Each p As SqlParameter In cmd.Parameters
            cmd1.Parameters().Add(CloneParam(p))
        Next
        Return cmd1
    End Function
    Public Shared Function GetNewVirtualBarcode(ByVal table As DataTable, ByVal fieldName As String) As String
        Dim rows As DataRow() = table.Select(String.Format("{0} LIKE '(new%'", fieldName), fieldName + " DESC")
        If rows.Length > 0 Then
            Try
                Dim maxNum As Integer = 0
                For Each row As DataRow In rows
                    Dim numStr As String = Utils.Str(row(fieldName)).Substring(4).Trim.Replace(")", "")
                    Dim num As Integer = CInt(numStr)
                    If num > maxNum Then
                        maxNum = num
                    End If
                Next
                Dim newNum As String = String.Format("(new {0})", maxNum + 1)
                Return newNum
            Catch ex As Exception
                Dbg.Debug("Error during virtual barcode creation: {0}", ex)
            End Try
        End If
        Return "(new 1)"
    End Function

    Public Overridable Sub Merge(ByVal dsTarget As DataSet, ByVal dsSource As DataSet)
#If Debug = True Then
        For Each t As DataTable In dsTarget.Tables
            Dbg.Assert(t.PrimaryKey IsNot Nothing AndAlso t.PrimaryKey.Length > 0, "The table {0} has no primary key", t.TableName)
        Next
#End If
        dsTarget.Merge(dsSource)
    End Sub

    Public Locked As Boolean = False
    Public Overridable Sub Lock _
        ( _
            ByVal lockingReason As String, _
            ByVal idfsLockedObject As Object, _
            ByVal idfsLogicalLockingObjectType As Object, _
            ByVal idfActivity As Object, _
            Optional ByVal dataSet As DataSet = Nothing _
        )
        Dim cmd As IDbCommand
        Dim adapter As DbDataAdapter
        Dim realLockedObject As Object

        realLockedObject = idfsLockedObject
        If (realLockedObject Is Nothing) Then
            realLockedObject = Me.ID
        End If

        cmd = CreateSPCommand("spLock")
        AddParam(cmd, "@idfsLogicalLockingStatus", "llsWriteLock", m_Error)
        AddParam(cmd, "@idfsLogicalLockingObjectType", idfsLogicalLockingObjectType, m_Error)
        AddParam(cmd, "@idfsLockedObject", realLockedObject, m_Error)
        AddParam(cmd, "@idfActivity", idfActivity, m_Error)
        AddParam(cmd, "@idfsLogicalLockingReason", lockingReason, m_Error)
        If (dataSet Is Nothing) Then
            AddParam(cmd, "@nMode", 0, m_Error)
            m_Error = ExecCommand(cmd, Nothing)
            If (Not m_Error Is Nothing) Then
                Throw m_Error.Exception
            End If
            Me.Locked = True
        Else
            AddParam(cmd, "@nMode", 1, m_Error)
            adapter = CreateAdapter(cmd)
            adapter.Fill(dataSet, "Lock")
            If (dataSet.Tables("Lock").Rows.Count > 0) Then
                Me.Locked = False
            Else
                Me.Locked = True
            End If
        End If
    End Sub

    Public Overridable Sub Unlock _
            ( _
                ByVal idfsLockedObject As Object, _
                ByVal idfsLogicalLockingObjectType As Object, _
                Optional ByVal idfActivity As Object = Nothing, _
                Optional ByVal force As Boolean = False _
            )
        Dim cmd As IDbCommand
        Dim realLockedObject As Object

        realLockedObject = idfsLockedObject
        If (realLockedObject Is Nothing) Then
            realLockedObject = Me.ID
        End If

        If ((force = True) OrElse (Me.Locked = True)) Then
            cmd = CreateSPCommand("spLock")
            AddParam(cmd, "@idfsLogicalLockingStatus", "llsUnlocked", m_Error)
            AddParam(cmd, "@idfsLockedObject", realLockedObject, m_Error)
            AddParam(cmd, "@idfsLogicalLockingObjectType", idfsLogicalLockingObjectType, m_Error)
            AddParam(cmd, "@idfActivity", idfActivity, m_Error)
            m_Error = ExecCommand(cmd, Nothing)
            If (Not m_Error Is Nothing) Then
                Throw m_Error.Exception
            End If
        End If
        Me.Locked = False
    End Sub
    Private Shared Sub AttachInfoMessageHandler(ByVal cn As IDbConnection)
        AddHandler CType(cn, SqlConnection).InfoMessage, New SqlInfoMessageEventHandler(AddressOf OnInfoMessage)
    End Sub

    Private Shared Sub OnInfoMessage(ByVal sender As Object, ByVal args As SqlInfoMessageEventArgs)
        Dim err As SqlError
        For Each err In args.Errors
            Trace.WriteLine(Trace.Kind.Info, "The {0} has received a severity {1}, state {2} error number {3}\n" & _
                              "on line {4} of procedure {5} on server {6}:\n{7}", _
                              err.Source, err.Class.ToString, err.State.ToString, err.Number.ToString, err.LineNumber.ToString, _
                              err.Procedure, err.Server, err.Message)
        Next
    End Sub

    Protected Overridable Sub ClearColumnsAttibutes(ByVal dataSet As DataSet)
        Dim table As DataTable
        Dim column As DataColumn

        For Each table In dataSet.Tables
            For Each column In table.Columns
                If Utils.Str(column.Expression) = "" Then
                    column.ReadOnly = False
                End If
                column.AllowDBNull = True
                column.AutoIncrement = False
            Next
        Next
    End Sub
    Private m_SkipValidation As Boolean = False
    Public Property SkipSqlValidation() As Boolean
        Get
            Return m_SkipValidation
        End Get
        Set(ByVal value As Boolean)
            m_SkipValidation = value
        End Set
    End Property

    Public Sub ClearEvents()
        If Not m_EventTypesToRaise Is Nothing Then
            m_EventTypesToRaise.Clear()
        End If

        For Each service As ServiceParam In m_LinkedServices
            service.service.ClearEvents()
        Next
    End Sub

    Private m_EventTypesToRaise As List(Of bv.common.Core.EventInfo)
    Public Function GetEventTypes() As List(Of bv.common.Core.EventInfo)
        If m_EventTypesToRaise Is Nothing Then
            m_EventTypesToRaise = New List(Of bv.common.Core.EventInfo)
        End If
        For Each service As ServiceParam In m_LinkedServices
            For Each evt As bv.common.Core.EventInfo In service.service.GetEventTypes
                AddEvent(evt.Type, evt.ID)
            Next
        Next
        Return m_EventTypesToRaise
    End Function
    Public Sub AddEvent(ByVal eventType As [Enum], Optional ByVal ID As Object = Nothing)
        If m_EventTypesToRaise Is Nothing Then
            m_EventTypesToRaise = New List(Of bv.common.Core.EventInfo)
        End If
        m_EventTypesToRaise.Add(New bv.common.Core.EventInfo(eventType, ID, 0))
    End Sub

#Region "Exec helper"

    'Alexander 10 oct 2008
    Public Shared Function ExecDataSet(ByVal cmd As IDbCommand) As DataSet
        Dim ds As New DataSet
        Dim Adap As DbDataAdapter = BaseDbService.CreateAdapter(cmd)
        Adap.Fill(ds)
        Return ds
    End Function

    'Alexander 10 oct 2008
    Public Shared Function ExecTable(ByVal cmd As IDbCommand) As DataTable
        Dim ds As DataSet = ExecDataSet(cmd)
        If (ds.Tables.Count = 0) Then Return Nothing

        Return ds.Tables(0)
    End Function

#End Region
    Private m_SearchProcedureName As String
    Public Overridable Property SearchProcedureName() As String
        Get
            Return m_SearchProcedureName
        End Get
        Set(ByVal value As String)
            m_SearchProcedureName = value
        End Set
    End Property
    Private m_SearchParameters As Generic.Dictionary(Of String, Object)
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SearchParameters() As Generic.Dictionary(Of String, Object)
        Get
            Return m_SearchParameters
        End Get
        Set(ByVal value As Generic.Dictionary(Of String, Object))
            m_SearchParameters = value
        End Set
    End Property
    Dim m_UseDatasetCopyInPost As Boolean = True
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Localizable(False), DefaultValue(True)> _
    Public Property UseDatasetCopyInPost() As Boolean
        Get
            If BaseDbService.copyDSinPost = False Then
                Return False
            Else
                Return m_UseDatasetCopyInPost
            End If
        End Get
        Set(ByVal value As Boolean)
            m_UseDatasetCopyInPost = value
        End Set
    End Property

    Shared m_IDCommand As IDbCommand
    Public Shared Function NewIntID(Optional ByVal transaction As IDbTransaction = Nothing) As Int64
        If m_IDCommand Is Nothing Then
            m_IDCommand = BaseDbService.CreateSPCommand("spsysGetNewID", ConnectionManager.DefaultInstance.Connection)
        End If
        SyncLock m_IDCommand
            If transaction Is Nothing Then
                m_IDCommand.Connection = ConnectionManager.DefaultInstance.Connection
            Else
                m_IDCommand.Connection = transaction.Connection
                m_IDCommand.Transaction = transaction
            End If
            If (m_IDCommand.Parameters.Count = 0) Then
                StoredProcParamsCache.CreateParameters(m_IDCommand)
            End If
        End SyncLock
        If m_IDCommand.Connection.State <> ConnectionState.Open Then
            m_IDCommand.Connection.Open()
        End If
        m_IDCommand.ExecuteNonQuery()
        Return Convert.ToInt64(CType(m_IDCommand.Parameters(0), SqlParameter).Value)
    End Function


    Public Shared Function NewListIntID(ByVal count As Int32, Optional ByVal transaction As IDbTransaction = Nothing) As List(Of Int64)
        Dim cmd As IDbCommand = CreateSPCommand("spsysGetListNewID", ConnectionManager.DefaultInstance.Connection)

        If transaction Is Nothing Then
            cmd.Connection = ConnectionManager.DefaultInstance.Connection
        Else
            cmd.Connection = transaction.Connection
            cmd.Transaction = transaction
        End If

        Dim params As New Dictionary(Of String, Object)
        params.Add("@Cnt", count)
        StoredProcParamsCache.CreateParameters(cmd, params)

        If cmd.Connection.State <> ConnectionState.Open Then
            cmd.Connection.Open()
        End If

        Dim result As List(Of Int64) = New List(Of Long)()
        Using reader As IDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim id As Long = reader.GetInt64(0)
                result.Add(id)
            End While
        End Using
        Return result

    End Function

    Public Shared Sub BindParamsToColumns(ByVal cmd As IDbCommand, ByVal sourceTable As DataTable)
        For Each param As IDataParameter In cmd.Parameters
            If (param.ParameterName.ToLowerInvariant = "@action") OrElse (param.ParameterName.ToLowerInvariant = "@return_value") Then
                Continue For
            End If
            If (param.ParameterName.ToLowerInvariant = "@LangID") OrElse (param.ParameterName.ToLowerInvariant = "@languageid") Then
                param.Value = bv.model.Model.Core.ModelUserContext.CurrentLanguage
                Continue For
            End If

            If sourceTable.Columns.Contains(param.ParameterName.Substring(1)) Then
                param.SourceColumn = param.ParameterName.Substring(1)
            Else
                Dbg.Debug("parameter {0} has no correspondent column in table {1}", param.ParameterName, sourceTable.TableName)
            End If
        Next
    End Sub

    Private Shared Function CreatePostDataAdapter(ByVal postProcedureName As String, ByVal sourceTable As DataTable, ByVal connection As IDbConnection, ByVal transaction As IDbTransaction, Optional ByVal params As Dictionary(Of String, Object) = Nothing, Optional continueUpdateOnError As Boolean = False) As DbDataAdapter
        Dim cmd As IDbCommand = CreateSPCommand(postProcedureName, connection, transaction)
        StoredProcParamsCache.CreateParameters(cmd, params)
        BindParamsToColumns(cmd, sourceTable)
        Dim da As DbDataAdapter = New SqlDataAdapter(CType(cmd, SqlCommand))
        If continueUpdateOnError Then
            da.ContinueUpdateOnError = True
        End If
        If cmd.Parameters.Contains("@Action") Then
            CType(da, IDbDataAdapter).InsertCommand = CloneCommand(cmd)
            SetParam(CType(da, IDbDataAdapter).InsertCommand, "@Action", DataRowState.Added)
            CType(da, IDbDataAdapter).DeleteCommand = CloneCommand(cmd)
            SetParam(CType(da, IDbDataAdapter).DeleteCommand, "@Action", DataRowState.Deleted)
            CType(da, IDbDataAdapter).UpdateCommand = CloneCommand(cmd)
            SetParam(CType(da, IDbDataAdapter).UpdateCommand, "@Action", DataRowState.Modified)
        Else
            CType(da, IDbDataAdapter).InsertCommand = cmd
            CType(da, IDbDataAdapter).DeleteCommand = cmd
            CType(da, IDbDataAdapter).UpdateCommand = cmd
        End If
        Return da
    End Function
    Public Shared Sub ExecPostProcedure(ByVal postProcedureName As String, ByVal sourceTable As DataTable, ByVal connection As IDbConnection, ByVal transaction As IDbTransaction, Optional ByVal params As Dictionary(Of String, Object) = Nothing, Optional ByVal deleteSort As String = Nothing, Optional ByVal filter As String = "", Optional rowUpdated As SqlRowUpdatedEventHandler = Nothing, Optional continueUpdateOnError As Boolean = False)
        Dim da As DbDataAdapter = CreatePostDataAdapter(postProcedureName, sourceTable, connection, transaction, params, continueUpdateOnError)
        AddHandler sourceTable.RowDeleted, AddressOf RowDeleted
        AddHandler sourceTable.RowChanged, AddressOf RowChanged

        Update(da, sourceTable, deleteSort, transaction, filter, rowUpdated)
    End Sub

    Private Shared Sub RowChanged(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)

    End Sub

    Private Shared Sub RowDeleted(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)

    End Sub


    Public Shared Sub ExecPostProcedure(ByVal postProcedureName As String, ByVal sourceRow As DataRow, ByVal connection As IDbConnection, ByVal transaction As IDbTransaction, Optional ByVal params As Dictionary(Of String, Object) = Nothing, Optional continueUpdateOnError As Boolean = False)
        Dim da As DbDataAdapter = CreatePostDataAdapter(postProcedureName, sourceRow.Table, connection, transaction, params, continueUpdateOnError)
        ApplyTransaction(da, transaction)
        da.Update(New DataRow() {sourceRow})
    End Sub
    Public Shared Sub PrintSQLCommandError(ByVal cmd As IDbCommand, ByVal e As Exception)
        Dbg.Debug("error during sql command execution:")
        Dbg.Debug("  command:{0}", cmd.CommandText)
        Dbg.Debug("  parameters: {0}", cmd.Parameters.Count)
        For Each p As SqlParameter In cmd.Parameters
            Dbg.Debug("    {0}:{1}", p.ParameterName, p.Value)
        Next
        PrintException(e)
        If Not e.InnerException Is Nothing Then
            Dbg.Debug("  Inner Exception:")
            PrintException(e.InnerException)
        End If
    End Sub
    Private Shared Sub PrintException(ByVal e As Exception)
        Dbg.Debug("  Exception Message:{0}", e.Message)
        If TypeOf e Is SqlException Then
            Dbg.Debug("  Sql Exception Details:")
            Dbg.Debug("     Number:{0}", CType(e, SqlException).Number)
            Dbg.Debug("     Class:{0}", CType(e, SqlException).Class)
            Dbg.Debug("     Procedure:{0}", CType(e, SqlException).Procedure)
            Dbg.Debug("     Line Number:{0}", CType(e, SqlException).LineNumber)
            Dbg.Debug("     SQL Exception Errors: {0}", CType(e, SqlException).Errors.Count)
            For Each err As SqlError In CType(e, SqlException).Errors
                Dbg.Debug("         Error message: {0}", err.Message)
                Dbg.Debug("         Error Number: {0}", err.Number)
                Dbg.Debug("         Error Class: {0}", err.Class)
                Dbg.Debug("         Error procedure: {0}", err.Procedure)
                Dbg.Debug("         Error LineNumber: {0}", err.LineNumber)
            Next
        End If
        Dbg.Debug("  Stack Trace:")
        Dbg.Debug(e.StackTrace)
    End Sub
End Class
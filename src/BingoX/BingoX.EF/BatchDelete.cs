#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BingoX.EF
{
    public class BatchDelete
    {
        /// <summary>The command text template.</summary>
        internal const string CommandTextTemplate = @"
DELETE
FROM    A {Hint}
FROM    {TableName} AS A
        INNER JOIN ( {Select}
                    ) AS B ON {PrimaryKeys}
SELECT @@ROWCOUNT
";
        internal const string CommandTextSqlCeTemplate = @"
DELETE
FROM    {TableName}
WHERE EXISTS ( SELECT 1 FROM ({Select}) AS B
               WHERE {PrimaryKeys}
           )
";
        internal const string CommandTextOracleTemplate = @"
DELETE
FROM    {TableName}
WHERE EXISTS ( SELECT 1 FROM ({Select}) B
               WHERE {PrimaryKeys}
           )
";

        internal const string CommandTextOracleTemplateCore = @"
DELETE
FROM    {TableName} A
WHERE EXISTS ( SELECT 1 FROM ({Select}) B
               WHERE {PrimaryKeys}
           )
";

        internal const string CommandTextSQLiteTemplate = @"
DELETE
FROM    {TableName}
WHERE EXISTS ( SELECT 1 FROM ({Select}) B
               WHERE {PrimaryKeys}
           )
";

        /// <summary>The command text postgre SQL template.</summary>
        internal const string CommandTextPostgreSQLTemplate = @"
DELETE FROM {TableName} AS A
USING ( {Select} ) AS B WHERE {PrimaryKeys}
";

        /// <summary>The command text MySQL template.</summary>
        internal const string CommandTextTemplate_MySql = @"
DELETE A
FROM {TableName} AS A
INNER JOIN ( {Select} ) AS B ON {PrimaryKeys}
";

        /// <summary>The command text Hana template.</summary>
        internal const string CommandTextTemplate_Hana = @"
DELETE
FROM    {TableName}
WHERE EXISTS ( SELECT 1 FROM ({Select}) B
               WHERE {PrimaryKeys}
           )
";

        /// <summary>The command text template with WHILE loop.</summary>
        internal const string CommandTextWhileTemplate = @"
DECLARE @stop int
DECLARE @rowAffected INT
DECLARE @totalRowAffected INT
SET @stop = 0
SET @totalRowAffected = 0
WHILE @stop=0
    BEGIN
        DELETE TOP ({Top})
        FROM    A {Hint}
        FROM    {TableName} AS A
                INNER JOIN ( {Select}
                           ) AS B ON {PrimaryKeys}
        SET @rowAffected = @@ROWCOUNT
        SET @totalRowAffected = @totalRowAffected + @rowAffected
        IF @rowAffected < {Top}
            SET @stop = 1
    END
SELECT  @totalRowAffected
";

        /// <summary>The command text template with DELAY and WHILE loop</summary>
        internal const string CommandTextWhileDelayTemplate = @"
DECLARE @stop int
DECLARE @rowAffected INT
DECLARE @totalRowAffected INT
SET @stop = 0
SET @totalRowAffected = 0
WHILE @stop=0
    BEGIN
        IF @rowAffected IS NOT NULL
            BEGIN
                WAITFOR DELAY '{Delay}'
            END
        DELETE TOP ({Top})
        FROM    A {Hint}
        FROM    {TableName} AS A
                INNER JOIN ( {Select}
                           ) AS B ON {PrimaryKeys}
        SET @rowAffected = @@ROWCOUNT
        SET @totalRowAffected = @totalRowAffected + @rowAffected
        IF @rowAffected < {Top}
            SET @stop = 1
    END
SELECT  @totalRowAffected
";

        /// <summary>Default constructor.</summary>
        public BatchDelete()
        {
            BatchSize = 4000;
        }

        /// <summary>Gets or sets the size of the batch.</summary>
        /// <value>The size of the batch.</value>
        public int BatchSize { get; set; }

        /// <summary>Gets or sets the batch delay interval in milliseconds (The wait time between batch).</summary>
        /// <value>The batch delay interval in milliseconds (The wait time between batch).</value>
        public int BatchDelayInterval { get; set; }

        /// <summary>Gets or sets a value indicating whether the query use table lock.</summary>
        /// <value>True if use table lock, false if not.</value>
        public bool UseTableLock { get; set; }

        /// <summary>Gets or sets the DbCommand before being executed.</summary>
        /// <value>The DbCommand before being executed.</value>


        /// <summary>Executes the batch delete operation.</summary>
        /// <typeparam name="T">The type of elements of the query.</typeparam>
        /// <param name="query">The query used to execute the batch operation.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute<T>(IQueryable<T> query) where T : class
        {


            string expression = query.Expression.ToString();

            if (Regex.IsMatch(expression, @"\.Where\(\w+ => False\)"))
            {
                return 0;
            }



            var dbContext = query.GetDbContext();
            var entity = dbContext.Model.FindEntityType(typeof(T));



            // CREATE command
           
            var connection = dbContext.Database.GetDbConnection();
            var command = connection.CreateCommand();

            // EXECUTE



            if (dbContext.Database.GetDbConnection().State != ConnectionState.Open)
            {

                dbContext.Database.OpenConnection();
            }



            if (command.GetType().Name == "NpgsqlCommand")
            {
                command.CommandText = command.CommandText.Replace("[", "\"").Replace("]", "\"");
                int totalRowAffecteds = command.ExecuteNonQuery();
                return totalRowAffecteds;
            }
            else if (command.Connection.GetType().Name.Contains("MySql"))
            {
                int totalRowAffecteds = command.ExecuteNonQuery();
                return totalRowAffecteds;
            }
            else if (command.GetType().Name.Contains("Sqlite"))
            {
                int totalRowAffecteds = command.ExecuteNonQuery();
                return totalRowAffecteds;
            }
            else if (command.GetType().Name.Contains("Oracle"))
            {
                int totalRowAffecteds = command.ExecuteNonQuery();
                return totalRowAffecteds;
            }

            var rowAffecteds = (int)command.ExecuteScalar();
            return rowAffecteds;


        }

 

        public string EscapeName(string name, bool isMySql, bool isOracle, bool isHana)
        {
            return isMySql ? string.Concat("`", name, "`") :
                isOracle || isHana ? string.Concat("\"", name, "\"") :
                    string.Concat("[", name, "]");
        }
    }
    internal static partial class InternalExtensions
    {
        public static DbContext GetDbContext<T>(this IQueryable<T> source)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(source.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (RelationalQueryContextFactory)queryContextFactoryField.GetValue(compiler);


            IStateManager stateManager = null;

            var dependenciesProperty = typeof(RelationalQueryContextFactory).GetField("_dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dependenciesProperty != null)
            {
                // EFCore 2.x
                var dependencies = (QueryContextDependencies)dependenciesProperty.GetValue(queryContextFactory);
                stateManager = dependencies.StateManager;
               
            }


          

            return stateManager.Context;
        }


    }
}

#endif
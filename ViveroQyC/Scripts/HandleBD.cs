using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ViveroQyC.Scripts
{
    public static class HandleBD
    {
        // Estructura para guardar los datos de las columnas
        public struct cColInfo
        {
            public String nombre; // Nombre de la columna.
            private Type dataType; // Tipo de dato de la columna.
            private bool isNull;    // Admite null o no.

            public cColInfo(string _nombre, Type _dataType, bool _isNUll)
            { nombre = _nombre; dataType = _dataType; isNull = _isNUll ; }

            public Type DataType
            {
                get
                {
                    return dataType;
                }

                set
                {
                    dataType = value;
                }
            }

            public bool IsNull
            {
                get
                {
                    return isNull;
                }

                set
                {
                    isNull = value;
                }
            }

            public string Nombre
            {
                get
                {
                    return nombre;
                }

                set
                {
                    nombre = value;
                }
            }

            
        }

        // Variables
        static int cantidad_tablas = -1;
        static List<string> lNombreTablas = new List<string>();                     // Lista para almacenar los nombres de las tablas.
        private static List<cColInfo> lDatosColumnas = new List<cColInfo>();        // Lista para almacenar los datos de las columnas.
                                                                                    //static List<string> nombre_columnas = new List<string>();
                                                                                    // Variables comandos 
        private enum eComandos { SELECT, INSERT, UPDATE, DELETE };                                                                
        static string selectall = "SELECT * FROM ";
        static SqlConnection conn = new SqlConnection(@"Data Source=GOMEZOMIL-PC\SQLEXPRESS1;Initial Catalog=viveroBD2;Integrated Security=True;Pooling=False");


        /*
         * Obtengo la cantidad de tablas que tiene la base de datos. 
        */
        public static void CargarCantidadTablas()
        {
            
            string comando = "SELECT COUNT(*) FROM information_schema.tables";
            SqlCommand com = new SqlCommand(comando, conn);
            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                    cantidad_tablas = reader.GetInt32(0);
            }
            else
            {
                Console.WriteLine("Error: No hay ninguna fila.");
            }
            Console.WriteLine("Cantidad tablas: " + cantidad_tablas);
            com.Connection.Close();
        }

        public static void CargarNombreTablas()
        {
            /*
             * Obtengo el nombre de las tablas.
             */
            string comando = "SELECT TABLE_NAME as Tabla FROM Information_Schema.Tables";
            SqlCommand com = new SqlCommand(comando, conn);
            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                    lNombreTablas.Add(reader.GetString(0));
            }
            else
            {
                Console.WriteLine("Error: No hay ninguna fila.");
            }
            com.Connection.Close();
        }

        // Valida si una tabla existe
        public static bool IfTablaExiste(string tabla)
        {
            foreach (string tab in LNombreTablas)
            {
                if (tabla.Equals(tab))
                    return true;
            }
            return false;
        }

        /*** SELECT ***/
        // Selecciona todo de una tabla.
        public static DataTable SelectAll(string tabla)
        {
            // Si no existe la tabla
            if (!IfTablaExiste(tabla))
                return null;


            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand(selectall + tabla, conn);
            try
            {
                com.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                com.Connection.Close();
                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en 'SELECT * FROM " + tabla + "' .");
                com.Connection.Close();
                return null;
            }
        }

        /*** INSERT ***/
        public static int Insert(string tabla, params object[] inputs)
        {
            /*
            * @params  tabla: cadena que contiene el nombre de la tabla donde se va a realizar el INSERT.
            *          inputs: arreglo de objetos que se van a ser los que se van a insertar en la tabla.
            * @return  1 : Si el INSERT fue exitoso.
            *          Otro: Si hubo algun problema o no cumple con alguna condicion. 
            */

            if (!IfTablaExiste(tabla))
                return 102;
            try
            {
                GetColData(tabla);

                int tipoDatos = 0;
                if ((tipoDatos = ComprobarTiposDatos(inputs)) != 1)
                {
                    return tipoDatos;
                }

                string comando = "insert into " + tabla + " ( ";

                // Creando una cadena "INSERT INTO table (value1, value2,...)
                foreach (cColInfo item in LDatosColumnas.Skip(1))
                {
                    comando += item.Nombre + ",";
                }
                comando = comando.Remove(comando.Length-1);
                comando += " ) VALUES ( '";

                //Agregando los valores a insertar en la cadena.
                foreach (object obj in inputs)
                {
                    comando += obj + "','";
                }

                comando = comando.Remove(comando.Length-1);
                comando = comando.Remove(comando.Length - 1);
                comando += " )";

                SqlCommand com = new SqlCommand(comando, conn);
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 100;
            }
        }

        public static int InsertSinCheckear(string tabla, params object[] inputs)
        {
            /*
             * ESTA FUNCION INSERTA VALORES EN UNA TABLA, SIN CHCKEAR SI LOS DATOS SON VALIDOS.
             * @params  tabla: cadena que contiene el nombre de la tabla donde se va a realizar el INSERT.
             *          inputs: arreglo de objetos que se van a ser los que se van a insertar en la tabla.
             * @return  1 : Si el INSERT fue exitoso.
             *          Otro: Si hubo algun problema o no cumple con alguna condicion. 
             */

            if (!IfTablaExiste(tabla))
                return 102;
            try
            {
                GetColData(tabla);

                string comando = "insert into " + tabla + " ( ";

                // Creando una cadena "INSERT INTO table (value1, value2,...)
                foreach (cColInfo item in LDatosColumnas.Skip(1))
                {
                    comando += item.Nombre + ",";
                }
                comando = comando.Remove(comando.Length - 1);
                comando += " ) VALUES ( '";

                //Agregando los valores a insertar en la cadena.
                foreach (object obj in inputs)
                {
                    comando += obj + "','";
                }

                comando = comando.Remove(comando.Length - 1);
                comando = comando.Remove(comando.Length - 1);
                comando += " )";

                SqlCommand com = new SqlCommand(comando, conn);
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 100;
            }
        }


        /*** UPDATE ***/
        public static int Update(string tabla, ushort id, params object[] inputs)
        {
            /*
            * @params  tabla: Tabla a la cual se le va a realizar el UPDATE.
            *          id: Clave primaria del registro al cual se le va a realizar el UPDATE.
            *          inputs: Arreglo con los datos que se usan para realizar el UPDATE.
            * @return  1: Si el UPDATE fue exitoso.
            *          Otro: Si hubo algun problema o no se cumplio alguna condicion.
            * @condiciones  tabla tiene que existir.
            *               Id debe ser de algun registro que exista.
            *               Comprobar que el tipo de datos corresponde con los atributos de la tabla.
            */

            if (!IfTablaExiste(tabla))
                return 102;

            try
            {
                GetColData(tabla);
                
                if (VerificarRegistro(tabla,id))
                {
                    // Si existe el registro
                    string comando = "UPDATE " + tabla + " SET ";

                    int tipoDatos = 0;
                    if ((tipoDatos = ComprobarTiposDatos(inputs)) != 1)
                    {
                        return tipoDatos;
                    }

                    comando += FormatearCadena(tabla, eComandos.UPDATE, inputs);
                    SqlCommand com = new SqlCommand(comando, conn);
                    com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                    return 1;

                }
                else
                {
                    return 103;
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public static int UpdateSinCheckear(string tabla, ushort id, params object[] inputs)
        {
            /*
             * ESTA FUNCION NO CHECKEA LA VALIDEZ DE LOS DATOS A INGRESAR.
             * @params  tabla: Tabla a la cual se le va a realizar el UPDATE.
             *          id: Clave primaria del registro al cual se le va a realizar el UPDATE.
             *          inputs: Arreglo con los datos que se usan para realizar el UPDATE.
             * @return  1: Si el UPDATE fue exitoso.
             *          Otro: Si hubo algun problema o no se cumplio alguna condicion.
             * @condiciones tabla tiene que existir.
             *          Id debe ser de algun registro que exista.
             */

            if (!IfTablaExiste(tabla))
                return 102;

            try
            {
                GetColData(tabla);

                // Si existe el registro
                if (VerificarRegistro(tabla, id))
                {
                    string comando = "UPDATE " + tabla + " SET ";
                    
                    comando += FormatearCadena(tabla, eComandos.UPDATE, inputs);
                    SqlCommand com = new SqlCommand(comando, conn);
                    com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                    return 1;

                }
                else
                {
                    return 103;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Comprueba que los tipos de datos esten bien.
        private static ushort ComprobarTiposDatos(object[] inputs)
        {
            /*
             * SOLO USAR SI SE COMPROBO LA EXISTENCIA DE LA TABLA.
             * 
             * Comprueba los dataType de lDatosColumna contra los tipos de dato de inputs
             * @return 1 si los tipos de datos concuerdan, 101 si algun tipo de dato no concuerda.
             */

            int index = 0;
            foreach (cColInfo item in lDatosColumnas.Skip(1))
            {
                if (!item.DataType.Equals(inputs[index].GetType()))
                    return 101;
                index++;
            }

            return 1;
        }

        // Verifica si un registro existe o no.
        private static bool VerificarRegistro(string tabla,  ushort id)
        {
            /*
             * @entrada
             * tabla :  nombre  de la tabla donde se va a realizar la verificacion.
             * id : id especificando el registro que se quiere comprobar. Entero positivo
             * @return TRUE si existe, 
             *         FALSE si no existe, o si la tabla no existe 
             */

            if (IfTablaExiste(tabla))
                return false;

            GetColData(tabla);
            SqlCommand comCount = new SqlCommand("SELECT count(*) FROM " + tabla + " WHERE " + LDatosColumnas[0].Nombre + "=" + id, conn);

            comCount.Connection.Open();
            int count = (int)comCount.ExecuteScalar();
            comCount.Connection.Close();

            if (count > 0)
                return true;
            else
                return false;

        }

        // Crea una cadena para usarla en un SQLCommand
        private static string FormatearCadena(string tabla, eComandos x, object[] inputs)
        {
            /*
             * LLamar a este metodo solo si se verifico que existe la 'tabla' antes. 
             */
            string comando = "";
            int index = 0;
            if (x == eComandos.INSERT)
            {
                comando += "INSERT INTO "+ tabla;
                foreach (cColInfo item in LDatosColumnas.Skip(1))
                {
                    
                }
            }

            if (x == eComandos.UPDATE)
            {
                foreach (cColInfo item in LDatosColumnas.Skip(1))
                {
                    comando += item.Nombre + " = " + inputs[index] + ", ";
                    index++;
                }
                comando = comando.Remove(comando.Length - 1);
                comando = comando.Remove(comando.Length - 1);
            }
            return comando;
        }

        // Obtiene la informacion de las columnas de una tabla.
        public static void GetColData(string tabla)
        {
            if (!IfTablaExiste(tabla))
                return;

            lDatosColumnas.Clear();     // Limpio la lista para usarla nuevamente.
            DataTable schema = null;
            /*using (var con = conn)
            {*/
                using (var schemaCommand = new SqlCommand("SELECT * FROM " + tabla, conn))
                {
                    conn.Open();
                    using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        schema = reader.GetSchemaTable();
                    }
                    conn.Close();
                }
            //}

            // Recorro las filas de schema, obtengo el nombre y el tipo de dato y lo guardo en la lista.
            foreach (DataRow row in schema.Rows)
            {
                // Guardo el nombre de la columna, el tipo de dato y si admite nulos o no. 
                cColInfo colInfo = new cColInfo(row.Field<String>("ColumnName"), Type.GetType(row["DataType"].ToString()), bool.Parse(row["AllowDBNull"].ToString()));
                LDatosColumnas.Add(colInfo);
            }
        }

        private static SqlCommand CargarComando(string comando)
        {
            SqlCommand com = new SqlCommand(comando, conn);
            com.Connection.Open();
            return com;
        }

        // Getters & Setters

        public static int Cantidad_tablas
        {
            get
            {
                return cantidad_tablas;
            }

            set
            {
                cantidad_tablas = value;
            }
        }

        public static List<string> LNombreTablas
        {
            get
            {
                return lNombreTablas;
            }

            set
            {
                lNombreTablas = value;
            }
        }

        public static List<cColInfo> LDatosColumnas
        {
            get
            {
                return lDatosColumnas;
            }

            set
            {
                lDatosColumnas = value;
            }
        }
    
    }
}

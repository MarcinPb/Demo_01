using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haestad.Domain;
using Haestad.Domain.ModelingObjects.Water;

namespace GeometryReader
{
    public sealed class DomainDataSetProxy : IDisposable
    {
        private readonly string filePath;
        private IdahoDataSource idahoDataSource;

        public DomainDataSetProxy(string filePath)
        {
            this.filePath = filePath;
        }

        public IDomainDataSet OpenDomainDataSet()
        {
            try
            {
                if (this.idahoDataSource == null)
                {
                    this.idahoDataSource = this.InitializeDataSource();
                }

                return this.idahoDataSource.DomainDataSetManager.DomainDataSet(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during opening the model database file: {0}.", ex.Message);
                throw;
            }
        }

        private IdahoDataSource InitializeDataSource()
        {
            var dataSource = new IdahoDataSource();
            dataSource.SetConnectionProperty(ConnectionProperty.FileName, this.filePath);
            dataSource.SetConnectionProperty(ConnectionProperty.ConnectionType, ConnectionType.Sqlite);
            dataSource.SetConnectionProperty(ConnectionProperty.EnableSchemaUpdate, false);
            dataSource.Open();
            return dataSource;
        }

        public void Dispose()
        {
            if (this.idahoDataSource != null && this.idahoDataSource.IsOpen())
            {
                this.idahoDataSource.Close();
            }

            this.idahoDataSource = null;
        }
    }
}

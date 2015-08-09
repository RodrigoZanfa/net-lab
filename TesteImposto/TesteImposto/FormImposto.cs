using Imposto.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imposto.Core.Domain;
using Imposto.Core.Util;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();

        public FormImposto()
        {
            InitializeComponent();

            #region [Alterações de componentes de tela]

            StartPosition = FormStartPosition.CenterScreen;

            cboEstadoOrigem.DataSource = EstadoUtil.CarregarUF();
            cboEstadoDestino.DataSource = EstadoUtil.CarregarUF();

            dataGridViewPedidos.AutoGenerateColumns = true;
            dataGridViewPedidos.DataSource = GetTablePedidos();
            ResizeColumns();

            #endregion
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)));

            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            if (ValidarCamposTela())
            {
                NotaFiscalService service = new NotaFiscalService();
                pedido.EstadoOrigem = cboEstadoOrigem.SelectedValue.ToString();
                pedido.EstadoDestino = cboEstadoDestino.SelectedValue.ToString();
                pedido.NomeCliente = txtNomeCliente.Text;

                DataTable table = (DataTable)dataGridViewPedidos.DataSource;

                foreach (DataRow row in table.Rows)
                {
                    pedido.ItensDoPedido.Add(
                        new PedidoItem()
                        {
                            NomeProduto = row["Nome do produto"].ToString(),
                            CodigoProduto = row["Codigo do produto"].ToString(),
                            ValorItemPedido = Convert.ToDouble(row["Valor"].ToString()),
                            Brinde = row.Field<bool?>("Brinde") ?? false  // Convert.ToBoolean(row["Brinde"])
                        });
                }

                try
                {
                    service.GerarNotaFiscal(pedido);

                    MessageBox.Show("Operação efetuada com sucesso", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimparTela();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCamposTela()
        {
            if (string.IsNullOrEmpty(txtNomeCliente.Text))
            {
                MessageBox.Show("Nome do Cliente inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeCliente.Focus();
                return false;
            }

            if (cboEstadoOrigem.SelectedValue == string.Empty)
            {
                MessageBox.Show("Estado de Origem inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEstadoOrigem.Focus();
                return false;
            }

            if (cboEstadoDestino.SelectedValue == string.Empty)
            {
                MessageBox.Show("Estado de Destino inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEstadoDestino.Focus();
                return false;
            }

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            if (table.Rows.Count == 0)
            {
                MessageBox.Show("Necessário informar ao menos 1 item para emitir Nota Fiscal", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimparTela()
        {
            cboEstadoOrigem.SelectedIndex = 0;
            cboEstadoDestino.SelectedIndex = 0;
            txtNomeCliente.Text = string.Empty;

            dataGridViewPedidos.DataSource = null;
            dataGridViewPedidos.DataSource = GetTablePedidos();
            ResizeColumns();

            txtNomeCliente.Focus();
        }

        private void buttonLimparDados_Click(object sender, EventArgs e)
        {
            LimparTela();
        }
    }
}

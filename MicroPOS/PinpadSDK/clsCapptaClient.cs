using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Cappta.Gp.Api.Com;
using Cappta.Gp.Api.Com.Model;
using MicroPOS.Helper;

namespace MicroPOS.PinpadSDK
{
	// Token: 0x0200006A RID: 106
	public class clsCapptaClient
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000ECA5 File Offset: 0x0000CEA5
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		public bool PendingState
		{
			get
			{
				return this._isPendingState;	
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000ECB5 File Offset: 0x0000CEB5
		public clsCapptaClient(MainWindow mainWindow)
		{
			this.m_mainWindow = mainWindow;
			this.m_cappta = new ClienteCappta();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public bool AuthenticatePdv()
		{
			PosHelper.Logger.Info("*****   AuthenticatePdv");
			string text = ConfigurationManager.AppSettings["ChaveAutenticacao"];
			if (string.IsNullOrWhiteSpace(text))
			{
				this._message = "Chave de Autenticação inválida";
				return false;
			}
			string text2 = ConfigurationManager.AppSettings["Cnpj"];
			if (string.IsNullOrWhiteSpace(text2) || text2.Length != 14)
			{
				this._message = "CNPJ inválido";
				return false;
			}
			int num;
			if (!int.TryParse(ConfigurationManager.AppSettings["Pdv"], out num) || num == 0)
			{
				this._message = "PDV inválido";
				return false;
			}
			PosHelper.Logger.Info(string.Format("AuthKey : {0}, CNPJ : {1}, PDV : {2}", text, text2, num));
			int num2 = this.m_cappta.AutenticarPdv(text2, num, text);
			if (num2 != 0)
			{
				this._message = this.GetErrorMessage(num2);
				PosHelper.Logger.Info(this._message);
				return false;
			}
			return true;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		public List<string> GetRechargeOperators()
		{
			IDetalhesOperadoras detalhesOperadoras = this.m_cappta.ObterOperadoras();
			if (detalhesOperadoras != null && detalhesOperadoras.Operadoras != null)
			{
				return detalhesOperadoras.Operadoras.ToList<string>();
			}
			PosHelper.Logger.Info("Operation List is Empty");
			return null;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000EE04 File Offset: 0x0000D004
		public bool ConfigrateIntegrationMode(bool p_bShow)
		{
			if (this.m_cappta == null)
			{
				this._message = "SDK is not initialized";
				return false;
			}
			IConfiguracoes configs = new Configuracoes
			{
				ExibirInterface = p_bShow
			};
			int num = this.m_cappta.Configurar(configs);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			return true;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000EE64 File Offset: 0x0000D064
		public List<DetalheLoja> GetShopList()
		{
			IRespostaLoja respostaLoja = this.m_cappta.ObterLojas();
			if (respostaLoja.CodigoResposta != 0)
			{
				this._message = this.GetErrorMessage(respostaLoja.CodigoResposta);
				PosHelper.Logger.Error(this._message);
				return null;
			}
			return respostaLoja.ListaLojas.ToList<DetalheLoja>();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		public bool PaymentDebit(double p_amount)
		{
			PosHelper.Logger.Info("*****   PaymentDebit : amount{p_amount}");
			int num = this.m_cappta.PagamentoDebito(p_amount);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			this._isPendingState = true;
			return true;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EF08 File Offset: 0x0000D108
		public bool PaymentCredit(double p_amount, int p_instNumber, InstallmentType p_instType, bool p_installment = true)
		{
			PosHelper.Logger.Info(string.Format("*****   PaymentCredit : amount={0}, parcelas={1}, type={2}, isInstallment={3}", new object[]
			{
				p_amount,
				p_instNumber,
				p_instType,
				p_installment
			}));
			IDetalhesCredito detalhes = new DetalhesCredito
			{
				QuantidadeParcelas = p_instNumber,
				TipoParcelamento = (int)p_instType,
				TransacaoParcelada = p_installment
			};
			int num = this.m_cappta.PagamentoCredito(p_amount, detalhes);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			this._isPendingState = true;
			return true;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
	
		
		
		
		public bool AbortOperation()
		{
			PosHelper.Logger.Info("*****   AbortOperation");
			if (!this._isPendingState)
			{
				return true;
			}
			int num = this.m_cappta.AbortarOperacao();
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			return true;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000F000 File Offset: 0x0000D200
		public bool PurchaseCredit(double p_amount, int p_instNumber)
		{
			PosHelper.Logger.Info("*****   PurchaseCredit");
			IDetalhesCrediario detalhes = new DetalhesCrediario
			{
				QuantidadeParcelas = p_instNumber
			};
			int num = this.m_cappta.PagamentoCrediario(p_amount, detalhes);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			this._isPendingState = true;
			return true;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F064 File Offset: 0x0000D264
		public bool ReprintLastCoupon(ReimpressaoType p_viaType)
		{
			int num = this.m_cappta.ReimprimirUltimoCupom((int)p_viaType);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			this._isPendingState = false;
			return true;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		public bool ReprintCoupon(ReimpressaoType p_viaType, string p_number)
		{
			PosHelper.Logger.Info(string.Format("*****   ReprintCoupon - type : {0}, num : {1}", p_viaType, p_number));
			int num = this.m_cappta.ReimprimirCupom(p_number, (int)p_viaType);
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			return true;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000F104 File Offset: 0x0000D304
		public bool Cancellation(string p_adminPswd, int p_number)
		{
			PosHelper.Logger.Info("*****   Cancellation");
			if (string.IsNullOrEmpty(p_adminPswd))
			{
				this._message = "A senha administrativa não pode ser vazia.";
				return false;
			}
			int num = this.m_cappta.CancelarPagamento(p_adminPswd, p_number.ToString("00000000000"));
			if (num != 0)
			{
				this._message = this.GetErrorMessage(num);
				PosHelper.Logger.Error(this._message);
				return false;
			}
			this._isPendingState = false;
			return true;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000F178 File Offset: 0x0000D378
		private string GetErrorMessage(int p_error)
		{
			string text = rcMessages.ResourceManager.GetString(string.Format("RESULTADO_CAPPTA_{0}", p_error));
			if (string.IsNullOrEmpty(text))
			{
				text = "Não foi possível executar a operação.";
			}
			return text;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		public bool FinishPayment()
		{
			PosHelper.Logger.Info("*****   FinishPayment");
			if (this.m_cappta == null)
			{
				return false;
			}
			if (!this._isPendingState)
			{
				return false;
			}
			this._isPendingState = false;
			int num = 1;
			if (num != 0)
			{
				PosHelper.Logger.Info("*****   ConfirmarPagamentos");
				this.m_cappta.ConfirmarPagamentos();
				return num != 0;
			}
			PosHelper.Logger.Info("*****   DesfazerPagamentos");
			this.m_cappta.DesfazerPagamentos();
			return num != 0;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000F222 File Offset: 0x0000D422
		public IIteracaoTef GetIterationTEF()
		{
			if (this.m_cappta == null)
			{
				return null;
			}
			return this.m_cappta.IterarOperacaoTef();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000F239 File Offset: 0x0000D439
		public void SendParameter(string p_parameter, int p_action)
		{
			if (this.m_cappta != null)
			{
				this.m_cappta.EnviarParametro(p_parameter, p_action);
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F251 File Offset: 0x0000D451
		private string GerarMensagemTransacaoAprovada()
		{
			return string.Format("Transação Aprovada!!! " + Environment.NewLine + " Clique em \"OK\" para confirmar a transação e \"Cancelar\" para desfaze-la.", Array.Empty<object>());
		}

		// Token: 0x04000280 RID: 640
		private MainWindow m_mainWindow;

		// Token: 0x04000281 RID: 641
		private ClienteCappta m_cappta;

		// Token: 0x04000282 RID: 642
		private bool _isPendingState;

		// Token: 0x04000283 RID: 643
		private string _message = "";
	}
}

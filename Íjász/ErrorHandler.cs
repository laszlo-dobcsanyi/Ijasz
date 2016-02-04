using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász {

    public enum ErrorCode {
        INDULO_CREATE,
        INDULO_MODIFY,
        INDULO_DELETE,
    }

    public class ErrorMessage {
        private string errorCode;
        private string errorMessage;
        private string exceptionMessage;

        private void showErrorMessage( ) {
            MessageBox.Show( errorMessage + Environment.NewLine + exceptionMessage, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return;
        }

        public ErrorMessage(ErrorCode errorCode, string _exceptionMessage ) {
            switch( errorCode ) {
                case ErrorCode.INDULO_CREATE:
                    errorMessage = "Hiba az induló létrehozásakor.";
                    break;
                default:
                    break;
            }
            exceptionMessage = _exceptionMessage;
            showErrorMessage( );
        }
    }
}

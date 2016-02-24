using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász {

    public enum ErrorCode {
        VERSENYSOROZAT_CREATE,
        VERSENYSOROZAT_MODIFY,

        INDULO_CREATE,
        INDULO_MODIFY,
    }

    public class ErrorMessage {
        private string errorCode;
        private string errorMessage;
        private string exceptionMessage;
        private Object errorObject;

        private void showErrorMessage( ) {
            MessageBox.Show( exceptionMessage + Environment.NewLine + Environment.NewLine + errorObject, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning ); return;
        }

        public ErrorMessage( ErrorCode errorCode, string _exceptionMessage, Object _errorObject = null ) {
            switch( errorCode ) {
                case ErrorCode.VERSENYSOROZAT_CREATE:
                    errorMessage = "Hiba a versenysorozat létrehozásakor.";
                    break;
                case ErrorCode.VERSENYSOROZAT_MODIFY:
                    errorMessage = "Hiba a versenysorozat módosításakor.";
                    break;

                case ErrorCode.INDULO_CREATE:
                    errorMessage = "Hiba az induló létrehozásakor.";
                    break;
                case ErrorCode.INDULO_MODIFY:
                    errorMessage = "Hiba az induló módosításakor.";
                    break;


                default:
                    break;
            }
            exceptionMessage = _exceptionMessage;
            errorObject = _errorObject;
            showErrorMessage( );
        }
    }
}

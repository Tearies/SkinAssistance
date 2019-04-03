#region HeaderInfo

// **********************************************************************************************************
// FileName:  IExecuteWithObject.cs
// Author:     Teries
// Email:  Teries@Tearies.com
// CopyRight:  2015© Tearies .All rights reserved.
// ChangeNote:
// 
// LastChange:   Teries
// ChangeDate:   2015-07-14-12:04
//  **********************************************************************************************************

#endregion

namespace SkinAssistance.Core.ICommands.Helper
{
    public interface IExecuteWithObject
    {
        #region Properties

        object Target { get; }

        #endregion

        void ExecuteWithObject(object parameter);
        void MarkForDeletion();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    bool CanExecute(object parameter);
    void Execute(object parameter);
}

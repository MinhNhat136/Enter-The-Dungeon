

public interface IModuleFactory<T_in, T_out>  
{
    T_out GetModule(T_in args);
}

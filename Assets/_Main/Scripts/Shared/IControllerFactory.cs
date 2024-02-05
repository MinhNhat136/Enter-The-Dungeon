

public interface IControllerFactory<T_in, T_out>  
{
    T_out GetController(T_in args);
}

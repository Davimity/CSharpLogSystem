# C# Log System (CLS)

[English readme](./README.md)

El gestor de registros (logs) de c# es un sistema de registro versátil diseñado para proyectos de C#, permite a los desarrolladores registrar mensajes en un archivo fácilmente con ciertas opciones de personalización. Este README (en español) ofrece un visión general del LogManager, sus características y como usar de forma efectiva esta herramienta en tus proyectos.

# Tabla de contenidos

1. Vista general
2. Uso
   
    · Uso básico

    · Personalización
   
3.Entendiendo el código

# 1. Vista general

Para usar el CLS en tu proyecto debes seguir los siguientes pasos simples:

1. Crea un objeto de tipo LogManager: Instancia un logManager en tu proyecto para poder manejar las operaciones. Esto es útil ya que permite que en un mismo código pueda haber varios LogManager, cada uno puede estar escribirendo en un archivo diferente o incluso escribir en el mismo archivo pero con distintas opciones. Para crear un objeto tipo LogManager:

```
LogManager manejador1 = new LogManager();
LogManager manejador2 = new LogManager();

//Esto crea 2 LogManager independientes, puediendo a partir de aquí modificar su configuración.
```

2. Registra mensajes: Usa el método Write() para escribir en tu archivo:

```
LogManager manejador = new LogManager();
manejador.Write("Esto es un Log!");
```

3. Opciones de personalización: Personaliza el comportamiento del sistema de registro modificando ciertos parámetros.

# 2. Uso

  # · Uso básico

  Para saber como utilizar el LogManager, primero se deben conocer los diferentes parámetros que se pueden modificar
  
  · logPath: un string que almacena el path en el que se van a almacenar los registros dentro de tu sistema. IMPORTANTE: debe incluir el nombre del archivo, por ejemplo:  C:\Users\usuario\AppData\LocalLow\Davimity\OpenPass\logs
  
  · writeTime: un bool que indica si se va a escribir la hora exacta del registro para cada registro del archivo
  
  · writeType: un bool que indica si se va a escribir el tipo de registro para cada registro del archivo.
  
  · timeFormat: un string que permite indicar el formato de texto en el que quieres expresar la hora que se va a escribir al lado de cada registro en el archivo en caso de tener writeTimer a true.
  
  · maxLogsStores: un uint que tiene el objetivo de regular la cantidad de archivos de registro que puede haber en la carpeta seleccionada como carpeta para los registros, esto con el fin de no almacenar una gran cantidad de registros y ocupar mucho espacio, puede ser modificado en cualquier momento.
  
  · type: un enum que almacena los tipos de registros que puede haber, por defecto son {INFO, WARNING, ERROR, FATAL} pero se pueden añadir o quitar tipos a voluntad.

# 3. Entendiendo el código

Para poder modificar la herramienta a tu gusto es necesario que comprendas como funciona. A la hora de crear un objeto de tipo LogManager, en es constructor puedes no especificar nada o especificar múltiples opciones, para saber cuales se puede modificar se recomienda mirar el código de CLS. Una vez creado el LogManager lo único que podrás hacer además de usar getters y setters para leer o modificar configuraciones será ejecutar el método Write(string).

El método Write(string) recibe como parámetro un string que será la información que se escribirá en un nuevo registro dentro del archivo de registros. Pero no se escribe directamente sino que se introduce en una cola, de forma que si por algún motivo en algún momento se acumulan varios registros para ser escritos en la cola, se escribirán todos con un único StreamWriter son necesidad de abrir varios, ahorrando así tiempo.

ESTA HERRAMIENTA ES FÁCILMENTE UTILIZABLE EN UNITY Y ESTÁ PREPARADA PARA ELLO.

Siéntete libre de explorar el código, modificarlo para que se ajuste a tu proyecto. En caso de tener dudas o encontrar algún error no dudes en pedir ayuda. Feliz registro! 📝🚀

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hop.Framework.Core.Cache
{
    public interface IDistributedCacheProvider<TCachedType>
    {
        /// <summary>
        /// Obter um item de cache
        /// </summary>
        /// <param name="key">Chave do cache</param>
        /// <returns>Se encontrou o item, retorna o mesmo, senão retorna o valor default() do mesmo</returns>
        TCachedType GetItem(string key);

        /// <summary>
        /// Obter ou adiciona um item de cache
        /// </summary>
        /// <param name="key">Chave do cache</param>
        /// <param name="valueFactoryIfNotExists">Função de inicialização do item do cache (seed) se o mesmo não existir no cache</param>
        /// <param name="explicitExpiration">Se passado um valor não nulo, usará o valor passado para expirar o item em um tempo diferente do padrão do cache que o mesmo encontra-se</param>
        /// <returns>Se encontrou o item, retorna o mesmo, senão retorna o valor default() do mesmo</returns>
        TCachedType GetItem(string key, Func<TCachedType> valueFactoryIfNotExists, TimeSpan? explicitExpiration = null);

        /// <summary>
        /// Adiciona ou atualiza um item de cache
        /// </summary>
        /// <param name="key">Chave do cache</param>
        /// <param name="item">Valor do item de cache a ser utilizado</param>
        /// <param name="explicitExpiration">Se passado um valor não nulo, usará o valor passado para expirar o item em um tempo diferente do padrão do cache que o mesmo encontra-se</param>
        /// <returns>Se encontrou o item, retorna o mesmo, senão retorna o valor default() do mesmo</returns>
        TCachedType SetItem(string key, TCachedType item, TimeSpan? explicitExpiration = null);

        /// <summary>
        /// Remove um item de cache
        /// </summary>
        /// <param name="key">Chave do cache</param>
        /// <returns>Retorna true se conseguiu remover o item</returns>
        bool RemoveItem(string key);

        /// <summary>
        /// Efetua a limpeza de todas as chaves do cache
        /// </summary>
        void Clear();

        /// <summary>
        /// Indica se o cache foi inicializado com sucesso
        /// </summary>
        bool IsActive { get; }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameLogic.Navigation{

	/// <summary>
	/// 保存寻路数据的数据结构，当它需要数据或者返回数据的时候能返回固定大小的数据。
	/// </summary>
	public class NPool<T> : IEnumerable<T> where T : new()  {

		#region 数据结构
		/// <summary>
		/// 表示一个收集池的入口
		/// </summary>
		public struct NNode{
			/// <summary>
			/// 通过下表获取数据池中的数据
			/// </summary>
			internal int NodeIndex;

			/// <summary>
			/// 池中的数据项
			/// </summary>
			public T Item;
		}

		#endregion

		#region 私用属性
		/// <summary>
		/// 固定池
		/// </summary>
		private NNode[] pool;

		/// <summary>
		/// 各个池节点的可访问状态
		/// </summary>
		private bool[] active;

		/// <summary>
		/// 可用项目节点索引队列。
		/// </summary>
		private Queue<int> available;
		#endregion

		#region 外部可调用属性
		/// <summary>
		/// 可用池的数量
		/// </summary>
		/// <remarks>
		/// Retrieving this property is an O(1) operation.
		/// </remarks>
		public int AvailableCount
		{
			get { return available.Count; }
		}

		/// <summary>
		/// 正在使用池的数量
		/// </summary>
		public int ActiveCount
		{
			get { return pool.Length - available.Count; }
		}

		/// <summary>
		/// 容量池的大小
		/// </summary>
		/// <value>The capacity.</value>
		public int Capacity
		{
			get{return pool.Length;}
		}
		#endregion

		#region 构造函数
		public NPool(int Capacity)
		{
			if (Capacity <= 0) 
				throw new ArgumentOutOfRangeException(
					"Pool must contain at least one item.");

			pool = new NNode[Capacity];
			active = new bool[Capacity];
			available = new Queue<int> (Capacity);

			for (int i=0; i<Capacity; i++) {
				pool[i] = new NNode();
				pool[i].NodeIndex = i;
				pool[i].Item = new T();

				active[i] = false;
				available.Enqueue(i);
			}
		}
		#endregion

		#region public function
		/// <summary>
		/// Clear all item in avalable.
		/// </summary>
		public void Clear(){
			available.Clear ();
			for (int i=0; i< pool.Length; i++) {
				active[i] = false;
				available.Enqueue(i);
			}
		}

		/// <summary>
		/// 从队列中去一个数据出来。
		/// </summary>
		/// <returns>The node that is removed from the available Pool.</returns>
		/// <exception cref="InvalidOperationException">
		/// There are no available items in the Pool.
		/// </exception>
		/// <remarks>
		/// This method is an O(1) operation.
		/// </remarks>
		public NNode Get()
		{
			int nodeIndex = available.Dequeue();
			active[nodeIndex] = true;
			return pool[nodeIndex];
		}

		/// <summary>
		/// 将一个数据重新添加进队列
		/// </summary>
		/// <param name="item">The node to return to the available Pool.</param>
		/// <exception cref="ArgumentException">
		/// The node being returned is invalid.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The node being returned was not active.
		/// This probably means the node was previously returned.
		/// </exception>
		/// <remarks>
		/// This method is an O(1) operation.
		/// </remarks>
		public void Return(NNode item)
		{
			if (item.NodeIndex < 0 || item.NodeIndex > pool.Length) {
				throw new ArgumentException("Invalid item node!");
			}

			if (!active [item.NodeIndex]) {
				throw new InvalidOperationException("Attempt to return an inactive node.");
			}

			active [item.NodeIndex] = false;
			available.Enqueue (item.NodeIndex);
		}

		/// <summary>
		/// 替换队列中某一个node的数据
		/// </summary>
		/// <param name="item">The node whose item value is to be set.</param>
		/// <exception cref="ArgumentException">
		/// The node being returned is invalid.
		/// </exception>
		/// <remarks>
		/// This method is necessary to modify the value of a value type stored
		/// in the Pool.  It copies the value of the node's Item field into the
		/// Pool.
		/// This method is an O(1) operation.
		/// </remarks>
		public void SetItemValue(NNode item)
		{
			if ((item.NodeIndex < 0) || (item.NodeIndex > pool.Length))
			{
				throw new ArgumentException("Invalid item node.");
			}
			
			pool[item.NodeIndex].Item = item.Item;
		}

		/// <summary>
		/// 从指定位置开始拷贝一个可被使用的数据
		/// </summary>
		/// <param name="array">
		/// 将池中可被使用的数据拷贝进这个指定的数组
		/// </param>
		/// <param name="arrayIndex">
		/// The index in array at which copying begins.
		/// </param>
		/// <returns>The number of items copied.</returns>
		/// <remarks>
		/// This method is an O(n) operation, where n is the smaller of 
		/// capacity or the array length.
		/// </remarks>
		public int CopyTo(T[] array, int arrayIndex)
		{
			int index = arrayIndex;
			
			foreach (NNode item in pool)
			{
				if (active[item.NodeIndex])
				{
					array[index++] = item.Item;
					
					if (index == array.Length)
					{
						return index - arrayIndex;
					}
				}
			}
			
			return index - arrayIndex;
		}

		#endregion

		#region IEnumerator接口相关
		/// <summary>
		/// 遍历一遍池从中返回所有可被使用的数据
		/// </summary>
		/// <returns>Enumerator for the active items.</returns>
		/// <remarks>
		/// This method is an O(n) operation, 
		/// where n is Capacity divided by ActiveCount. 
		/// </remarks>
		public IEnumerator<T> GetEnumerator()
		{
			foreach (NNode item in pool)
			{
				if (active[item.NodeIndex])
				{
					yield return item.Item;
				}
			}
		}

		/// <summary>
		/// 返回池中所以可以被使用的node
		/// in the Pool.
		/// </summary>
		/// <remarks>
		/// This method is an O(n) operation, 
		/// where n is Capacity divided by ActiveCount. 
		/// </remarks>
		public IEnumerator<NNode> ActiveNodes
		{
			get{
				foreach(NNode item in pool){
					if(active[item.NodeIndex]){
						yield return item;
					}
				}

			}
		}

		/// <summary>
		/// 返回池中所以的node
		/// in the Pool.
		/// </summary>
		/// <remarks>
		/// This method is an O(n) operation, 
		/// where n is Capacity divided by ActiveCount. 
		/// </remarks>
		public IEnumerator<NNode> AllNodes
		{
			get{
				foreach(NNode item in pool){
					yield return item;
				}
				
			}
		}

		/// <summary>
		/// Implementation of the IEnumerable interface.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}

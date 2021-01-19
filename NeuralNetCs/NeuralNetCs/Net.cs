using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace NeuralNetCs
{
   
     class Net
    {
        public List<List<Neuron>> _neuron_net { get; set; }
      
        /// <summary>
        /// Creates neural net.
        /// </summary>
        /// <param name="net_schematic">Used to create specific neural net.</param>
        /// <param name="learning_speed">Used to set default learning speed for each neuron.</param>
        public Net(int[] net_schematic, double learning_speed)
        {
            _neuron_net = new List<List<Neuron>>();
            _create_net(net_schematic, learning_speed);
        }

        /// <summary>
        /// Creates nueral net. Used in Net's constructor.
        /// </summary>
        /// <param name="net_schematic">Used to create specific neural net.</param>
        /// <param name="learning_speed">Used to set default learning speed for each neuron.</param>
        private void _create_net(int[] net_schematic, double learning_speed)
        {
            for (int i = 0; i < net_schematic.Length; i++)
            {
                List<Neuron> tmp_list = new List<Neuron>();
                for (int j = 0; j < net_schematic[i]; j++)
                {
                    if (i == 0)
                    {
                        Neuron tmp_neuron = new Neuron(net_schematic[i], learning_speed);
                        tmp_list.Insert(j, tmp_neuron);
                    }
                    else
                    {
                        Neuron tmp_neuron = new Neuron(net_schematic[i - 1], learning_speed);
                        tmp_list.Insert(j, tmp_neuron);
                    }
                }

                _neuron_net.Insert(i, tmp_list);
            }
        }

        /// <summary>
        /// Propagates the signal from arguments through layers to the end.
        /// </summary>
        /// <param name="argument_vector">Neural net's data.</param>
        public void _propagate_the_signal(double[] argument_vector)
        {
            for (int i = 0; i < _neuron_net.Count; i++)
            {
                for (int j = 0; j < _neuron_net[i].Count; j++)
                {
                    if (i == 0)
                    {
                        _neuron_net[i][j].calculate_first_layer_output(argument_vector);
                    }
                    else
                    {
                        _neuron_net[i][j].calculate_output(_neuron_net.ElementAt(i - 1));
                    }
                }
            }
        }

        /// <summary>
        /// Calculates errors for each neurons.
        /// </summary>
        /// <param name="expected_value">Value at the end of the neural net.</param>
        private void _calculate_errors(double expected_value)
        {
            for (int i = _neuron_net.Count - 1; i >= 0; i--)
            {
                {
                    for (int j = 0; j < _neuron_net[i].Count; j++)
                    {
                        if (i == _neuron_net.Count - 1)
                        {
                            _neuron_net[i][j].calculate_last_layer_error(expected_value);
                        }
                        else
                        {
                            _neuron_net[i][j].calculate_error(_neuron_net.ElementAt(i + 1), j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes weight's for each neurons for minimalize errors.
        /// </summary>
        /// <param name="argument_vector">Neural net's data.</param>
        private void _weight_corrections(double[] argument_vector)
        {
            for (int i = 0; i < _neuron_net.Count; i++)
            {
                for (int j = 0; j < _neuron_net[i].Count; j++)
                {
                    if (i == 0)
                    {
                        _neuron_net[i][j].weight_correction_first_layer(argument_vector);
                    }
                    else if (i == 1)
                        _neuron_net[i][j].weight_correction(_neuron_net.ElementAt(i - 1));
                }
            }
        }
        /// <summary>
        /// Main neural net's procedure. Calls all functions.
        /// </summary>
        /// <param name="data_list">Neural net's data.</param>
        /// <param name="expected_values">Values at the end of the neural net.</param>
        /// <param name="amount_of_repeat">How many times neural net is going to learn.</param>
        public void learn(double[][] data_list, double[] expected_values, int amount_of_repeat)
        {
            for (int i = 0; i < amount_of_repeat; i++)
            {
                for (int j = 0; j < data_list.Length; j++)
                {
                    _propagate_the_signal(data_list[j]);

                    _calculate_errors(expected_values[j]);

                    _weight_corrections(data_list[j]);

                }
            }
        }
    }
}

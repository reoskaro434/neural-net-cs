using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetCs
{
    class Neuron
    {
        public double[] _weight_vector { get; set; }
        public double _last_output { get; set; }
        public double _last_error { get; set; }
        public double _learning_speed { get; set; }

        /// <summary>
        /// Creates new neuron.
        /// </summary>
        /// <param name="new_weight_amount">Number of the weights depends on number of arguments/neurons in left layer.</param>
        /// <param name="new_learning_speed">Tells, how fast the neuron is able to learn.</param>
        public Neuron(int new_weight_amount, double new_learning_speed)
        {
            Random rnd = new Random();
            _last_error = 0;
            _last_output = 0;
            _learning_speed = new_learning_speed;
            _weight_vector = new double[new_weight_amount];


            for (int i = 0; i < new_weight_amount; i++)
                _weight_vector[i] = rnd.NextDouble();
        }
        /// <summary>
        /// The activation function which is used in neural net.
        /// </summary>
        /// <param name="argument">The "x" argument.</param>
        /// <returns>Activation function's output.</returns>
        private double _activation_function(double argument)
        {
            return Math.Tanh(argument);
        }
        /// <summary>
        /// The derivative of activation function which is used in neural net.
        /// </summary>
        /// <param name="argument">The "x" argument.</param>
        /// <returns>Deriviative output.</returns>
        private double _derivative_of_activation_function(double argument)
        {
            return 1 - Math.Pow(_activation_function(argument), 2);
        }
        /// <summary>
        /// Takes the arguments, and propagate the signal through first layer.
        /// </summary>
        /// <param name="argument_vector">Array of arguments.</param>
        public void calculate_first_layer_output(double[] argument_vector)
        {
            double tmp_sum = 0;
            for (int i = 0; i < _weight_vector.Length; i++)
            {
                tmp_sum += argument_vector[i] * _weight_vector.ElementAt(i);
            }
            _last_output = _activation_function(tmp_sum);
        }
        /// <summary>
        /// Propagates outputs through layers.
        /// </summary>
        /// <param name="left_layer">Left layer of the neurons.</param>
        public void calculate_output(List<Neuron> left_layer)
        {
            double tmp_sum = 0;
            for (int i = 0; i < _weight_vector.Length; i++)
            {
                tmp_sum += left_layer.ElementAt(i)._last_output * _weight_vector.ElementAt(i);
            }
            _last_output = _activation_function(tmp_sum);
        }
        /// <summary>
        /// Propagates the error from last layer.
        /// </summary>
        /// <param name="expected_value">Current neuron's target value.</param>
        public void calculate_last_layer_error(double expected_value)
        {
            _last_error = _derivative_of_activation_function(_last_output) * (expected_value - _last_output);
        }
        /// <summary>
        /// Propagates the error through layers.
        /// </summary>
        /// <param name="right_layer">Layer from which the error is taken.</param>
        /// <param name="neuron_index">Current weight which is calculated in neuron.</param>
        public void calculate_error(List<Neuron> right_layer, int neuron_index)
        {
            double error_sum = 0;
            for (int i = 0; i < right_layer.Count; i++)
            {
                error_sum += _derivative_of_activation_function(_last_output) * right_layer.ElementAt(i).return_weight_multiplied_by_error(neuron_index);
            }
            _last_error = error_sum;
        }
        /// <summary>
        /// Multiplies neuron's error and weight. 
        /// </summary>
        /// <param name="neuron_index">Current neuron's index.</param>
        /// <returns>Multiplied neuron's error and weight.</returns>
        public double return_weight_multiplied_by_error(int neuron_index)
        {
            return _weight_vector.ElementAt(neuron_index) * _last_error;
        }
        /// <summary>
        /// Corrects weights from first layer.
        /// </summary>
        /// <param name="argument_vector">The data used to learn.</param>
        public void weight_correction_first_layer(double[] argument_vector)
        {
            for (int i = 0; i < _weight_vector.Length; i++)
            {
                _weight_vector[i] += argument_vector[i] * _learning_speed * _last_error;
            }
        }
        /// <summary>
        /// Corrects weights using left layer's data.
        /// </summary>
        /// <param name="left_layer">Left layer's neurons.</param>
        public void weight_correction(List<Neuron> left_layer)
        {
            for (int i = 0; i < _weight_vector.Length; i++)
            {
                _weight_vector[i] += left_layer.ElementAt(i)._last_output * _learning_speed * _last_error;
            }
        }
    }
}
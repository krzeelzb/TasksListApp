using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TasksListApp.Models;
using TasksListApp.Services;

namespace TasksListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly ILogger<TasksController> logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            this.taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<TasksController>
        /// <summary>
        /// Gets list of all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult<List<TaskItem>> Get()
        {
            var tasksList = taskService.GetTasks();
            if (tasksList.Count() == 0)
            {
                return NotFound($"Tasks list is empty.");
            }
            logger.LogInformation($"Tasks list successfully retrieved.");
            return tasksList;
            
        }

        // GET api/<TasksController>/5
        /// <summary>
        /// Returns task if the task of provided id exists
        /// </summary>
        /// <param name="id">Id of the task</param>
        /// <returns>Returns TaskItem</returns>
        [HttpGet("{id}")]
        public ActionResult<TaskItem> Get(string id)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));

            logger.LogInformation($"Trying to get task with Id = {id}.");
            TaskItem task = taskService.GetTask(id);
            if (task == null)
            {
                logger.LogError($"Task not found.");
                return NotFound($"Task with Id = {id} not found.");
            }

            logger.LogInformation($"Task with found.");
            return task;
        }

        // POST api/<TasksController>
        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="task"></param>
        [HttpPost]
        public ActionResult Post([FromBody] TaskItem task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            logger.LogInformation($"Trying to create new task task with {task.ToString()}");
            taskService.CreateTask(task);
            logger.LogInformation($"Successfuly proccessed.");
            return Ok(task);
        }

        // PUT api/<TasksController>/5
        /// <summary>
        /// Replaces task resource if the task of provided id exists
        /// </summary>
        /// <param name="id">Id of the task</param>
        /// <param name="task">TaskItem</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] TaskItem task)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));
            _ = task ?? throw new ArgumentNullException(nameof(task));

            logger.LogInformation($"Trying to replace task with Id = {id}.");
            TaskItem existingTask = taskService.GetTask(id);
            if (existingTask == null)
            {
                logger.LogError($"Task not found.");
                return NotFound($"Task with Id = {id} not found.");
            }
            taskService.UpdateTask(id, task);
            logger.LogInformation($"Successfuly proccessed.");
            return NoContent();
        }

        // Patch api/<TasksController>/5
        /// <summary>
        /// Update parts of the task resource if the task of provided id exists
        /// </summary>
        /// <param name="id">Id of the task</param>
        /// <param name="patch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPatch("{id}")]
        public ActionResult Patch(string id, [FromBody] JsonPatchDocument<TaskItem> patch)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));
            _ = patch ?? throw new ArgumentNullException(nameof(patch));

            logger.LogInformation($"Trying to update task with Id = {id}.");
            TaskItem existingTask = taskService.GetTask(id);
            if (existingTask == null)
            {
                logger.LogError($"Task not found.");
                return NotFound($"Task with Id = {id} not found.");
            }
            patch.ApplyTo(existingTask, ModelState);
            taskService.UpdateTask(id, existingTask);

            logger.LogInformation($"Successfuly proccessed.");
            return Ok(existingTask);
        }


        // DELETE api/<TasksController>/5
        /// <summary>
        /// Delete task resource if the task of provided id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));

            logger.LogInformation($"Trying to delete task with Id = {id}.");

            TaskItem existingTask = taskService.GetTask(id);
            if (existingTask == null)
            {
                logger.LogError($"Task not found.");
                return NotFound($"Task with Id = {id} not found.");
            }

            taskService.DeleteTask(id);
            logger.LogInformation($"Successfuly deleted.");
            return Ok($"Task with Id = {id} deleted.");
        }
    }
}

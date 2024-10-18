using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.Entity;
using ModelLayer.Model.Enum;

namespace BusinessLayer.ServiceImpl
{
    public class CollaborationBL : ICollaborationService
    {
        private readonly ICollaboration _collaborationRepo;

        public CollaborationBL(ICollaboration collaborationRepo)
        {
            _collaborationRepo = collaborationRepo;
        }


        public Task<ResponseBody<bool>> DeleteCollaborationAsync(int collaborationId)
        {
            return _collaborationRepo.DeleteCollaborationAsync(collaborationId);
        }

        public Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByNoteIdAsync(int noteId)
        {
            return _collaborationRepo.GetCollaborationsByNoteIdAsync(noteId);
        }

        public Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByUserIdAsync(int userId)
        {
            return _collaborationRepo.GetCollaborationsByUserIdAsync((int)userId);
        }

        public Task<ResponseBody<bool>> UpdateCollaborationAsync(int collaborationId, Role role)
        {
            return _collaborationRepo.UpdateCollaborationAsync((int)collaborationId, role);
        }

        public Task<ResponseBody<CollaboratorResponceDto>> CreateCollaborationAsync(CollaboratorRequestDto collaboratorRequestDto)
        {
            return _collaborationRepo.CreateCollaborationAsync(collaboratorRequestDto);
        }

        public Task<ResponseBody<bool>> RemoveCollaboratorAsync(int noteId, int collaboratorId)
        {
            return _collaborationRepo.RemoveCollaboratorAsync(noteId, (int)collaboratorId);
        }

        public Task<ResponseBody<bool>> AddCollaboratorAsync(int noteId, int collaboratorId, Role role)
        {
            return _collaborationRepo.AddCollaboratorAsync(noteId, collaboratorId, role);
        }

        public Task<Collaborator> GetCollaboratorByNoteAndUserAsync(int noteId, int userId)
        {
            return _collaborationRepo.GetCollaboratorByNoteAndUserAsync(noteId, (int)userId);
        }
    }
}
